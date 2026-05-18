using Application_Service.Common;
using Application_Service.DTO_s.FacultyDTO_s;
using Application_Service.DTO_s.StudentGradDTO_s;
using Application_Service.DTO_s.SubjectDTO_s;
using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Application_Service.Services.FacultyServices.Interfaces;
using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.FacultyServices.Implementations
{
    public class FacultyService : IFacultyService
    {
        private readonly IUnitOfWork _uow;
        public FacultyService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<ApiResponse<List<GetFacultyDTO>>> GetFacultiesByDepartmentAsync(Guid departmentId)
        {
            // Optional: check if department exists (recommended)
            var departmentExists = await _uow.DepartmentRepository
                .GetByIdAsync(departmentId);

            if (departmentExists == null)
            {
                return ApiResponse<List<GetFacultyDTO>>.Fail(
                    "Department not found.",
                    ResponseType.NotFound
                );
            }

            // Get faculty filtered by department
            var faculties = await _uow.FucaltyRepo
                .Query()
                .Include(f=> f.User)
                .Where(f => f.DepartmentId == departmentId)
                .OrderBy(f => f.JoiningDate)
                .ToListAsync();

            if (!faculties.Any())
            {
                return ApiResponse<List<GetFacultyDTO>>.Fail(
                    new List<GetFacultyDTO>(),
                    "No faculty found for this department.",
                    ResponseType.NotFound
                );
            }

            var result = faculties.Select(f => new GetFacultyDTO
            {
                FacultyId = f.FacultyId,
                UserId = f.UserId,
                DepartmentId = f.DepartmentId,
                Designation = f.Designation,
                JoiningDate = f.JoiningDate,
                FullName = f.User.FullName
            }).ToList();

            return ApiResponse<List<GetFacultyDTO>>.Success(
                result,
                "Faculty retrieved successfully.",
                ResponseType.Ok
            );
        }

        public async Task<ApiResponse<List<GetSubjectDto>>> GetMySubjectsAsync(Guid userId)
        {
            try
            {
                var faculty = await GetFacultyByUserId(userId);
                var facultyId = faculty.FacultyId;

                var subjects = await _uow.SubjectRepository.Query()
                    .Include(s => s.Semester)
                    .Include(s => s.Department)
                    .Include(s => s.Faculty)
                    .Where(s => s.FacultyId == facultyId)
                    .ToListAsync();

                var result = subjects.Select(s => new GetSubjectDto
                {
                    SubjectId = s.SubjectId,
                    Title = s.Title,
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.Department != null ? s.Department.Name : "",
                    SemesterId = s.SemesterId,
                    SemesterName = s.Semester != null ? s.Semester.Name : "",
                    FacultyId = s.FacultyId,
                    FacultyName = s.Faculty != null ? s.Faculty.Designation : "",
                    IsActive = true // agar field hai to adjust kar lena
                }).ToList();

                return ApiResponse<List<GetSubjectDto>>.Success(
                    result,
                    "Subjects retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<GetSubjectDto>>.Fail(
                    "Failed to get subjects",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<List<GetEnrolledStudentDto>>> GetEnrolledStudentsAsync(Guid subjectId,Guid userId)
        {
            try
            {
                var faculty = await GetFacultyByUserId(userId);
                var facultyId = faculty.FacultyId;

                // 🔹 Verify subject belongs to faculty
                var subject = await _uow.SubjectRepository.Query()
                    .FirstOrDefaultAsync(s =>
                        s.SubjectId == subjectId &&
                        s.FacultyId == facultyId);

                if (subject == null)
                {
                    return ApiResponse<List<GetEnrolledStudentDto>>.Fail(
                        "Subject not found or unauthorized access",
                        ResponseType.Forbidden);
                }

                var data = await (
                    from e in _uow.EnrollmentRepo.Query()
                    join s in _uow.StudentRepo.Query() on e.StudentId equals s.StudentId
                    join g in _uow.GradeRepo.Query() on e.EnrollmentId equals g.EnrollmentId into grades
                    from g in grades.DefaultIfEmpty()
                    where e.SubjectId == subjectId
                    select new GetEnrolledStudentDto
                    {
                        EnrollmentId = e.EnrollmentId,
                        StudentId = s.StudentId,
                        StudentName = s.StudentName,
                        RollNo = s.RollNo,

                        GradeId = g != null ? g.GradeId : null,
                        Grade = g != null ? g.Grad : null,
                        GradePoints = g != null ? g.GradePoints : null,

                        MidtermMarks = g != null ? g.MidtermMarks : null,
                        FinalMarks = g != null ? g.FinalMarks : null,
                        AssignmentMarks = g != null ? g.AssignmentMarks : null,
                        QuizMarks = g != null ? g.QuizMarks : null,
                        TotalMarks = g != null ? g.TotalMarks : null
                    }
                ).ToListAsync();

                return ApiResponse<List<GetEnrolledStudentDto>>.Success(
                    data,
                    "Students retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<GetEnrolledStudentDto>>.Fail(
                    "Failed to get students",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<string>> AssignGradeAsync(CreateOrUpdateGradeRequest request,Guid userId)
        {
            try
            {
                var faculty = await GetFacultyByUserId(userId);
                var facultyId = faculty.FacultyId;

                // 🔹 Get Enrollment with Subject validation
                var enrollment = await _uow.EnrollmentRepo.Query()
                    .Include(e => e.Subject)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == request.EnrollmentId);

                if (enrollment == null)
                {
                    return ApiResponse<string>.Fail(
                        "Enrollment not found",
                        ResponseType.NotFound);
                }

                // 🔹 Security check: only assigned faculty can grade
                if (enrollment.Subject.FacultyId != facultyId)
                {
                    return ApiResponse<string>.Fail(
                        "Unauthorized access",
                        ResponseType.Forbidden);
                }

                // 🔹 Calculate grade
                var (grade, points, total) = CalculateGrade(
                    request.MidtermMarks,
                    request.FinalMarks,
                    request.AssignmentMarks,
                    request.QuizMarks);

                var existing = await _uow.GradeRepo.Query()
                    .FirstOrDefaultAsync(g => g.EnrollmentId == request.EnrollmentId);

                if (existing != null)
                {
                    existing.MidtermMarks = request.MidtermMarks;
                    existing.FinalMarks = request.FinalMarks;
                    existing.AssignmentMarks = request.AssignmentMarks;
                    existing.QuizMarks = request.QuizMarks;
                    existing.TotalMarks = total;
                    existing.Grad = grade;
                    existing.GradePoints = points;

                    await _uow.GradeRepo.Update(existing);
                }
                else
                {
                    var newGrade = new Grade
                    {
                        GradeId = Guid.NewGuid(),
                        EnrollmentId = request.EnrollmentId,
                        MidtermMarks = request.MidtermMarks,
                        FinalMarks = request.FinalMarks,
                        AssignmentMarks = request.AssignmentMarks,
                        QuizMarks = request.QuizMarks,
                        TotalMarks = total,
                        Grad = grade,
                        GradePoints = points
                    };

                    await _uow.GradeRepo.CreateAsync(newGrade);
                }

                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success("Success",
                    "Grade saved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to assign grade",
                    ResponseType.InternalServerError);
            }
        }
        private (string grade, float gradePoints, int totalMarks) CalculateGrade(int mid, int final, int assignment, int quiz)
        {
            var total = mid + final + assignment + quiz;

            float percentage = (total / 60f) * 100f;

            if (percentage >= 85) return ("A", 4.0f, total);
            if (percentage >= 80) return ("A-", 3.7f, total);
            if (percentage >= 75) return ("B+", 3.3f, total);
            if (percentage >= 70) return ("B", 3.0f, total);
            if (percentage >= 65) return ("B-", 2.7f, total);
            if (percentage >= 60) return ("C+", 2.3f, total);
            if (percentage >= 55) return ("C", 2.0f, total);
            if (percentage >= 50) return ("D", 1.0f, total);

            return ("F", 0.0f, total);
        }


        private  async Task<Faculty> GetFacultyByUserId(Guid userId)
        {
            var faculty = await  _uow.FucaltyRepo.FirstOrDefaultAsync(x=> x.UserId == userId);

            return new Faculty
            {
                FacultyId = faculty.FacultyId,
                DepartmentId = faculty.DepartmentId,
                UserId = userId,
                JoiningDate = faculty.JoiningDate,
                Designation = faculty.Designation,
                SubjectsTaught = faculty.SubjectsTaught,
                SupervisedProjects = faculty.SupervisedProjects,
                User = faculty.User
            };

        }
    }
}
