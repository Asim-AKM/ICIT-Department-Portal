using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Services.StudentServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.StudentServices.Implementation
{
    public class TranscriptService : ITranscriptService
    {

        private readonly IUnitOfWork _uow;
        public TranscriptService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<ApiResponse<TranscriptDto>> GetStudentTranscriptAsync(Guid userId)
        {
            try
            {

                var studententity = await _uow.StudentRepo.GetStudentByUserId(userId);
                var studentId = studententity.StudentId;
                // 🔹 Validate Student Id
                if (studentId == Guid.Empty)
                {
                    return ApiResponse<TranscriptDto>.Fail(
                        "Invalid student identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Load Student
                var student = await _uow.StudentRepo.Query()
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student == null)
                {
                    return ApiResponse<TranscriptDto>.Fail(
                        "Student not found",
                        ResponseType.NotFound);
                }

                // 🔹 Load User (for FatherName if available)
                var user = await _uow.UserRepo.Query()
                    .FirstOrDefaultAsync(u => u.UserId == student.UserId);

                // 🔹 Load Department
                var department = await _uow.DepartmentRepository
                    .GetByIdAsync(student.DepartmentId);

                // 🔹 Load Session
                var session = await _uow.SessionRepo
                    .GetByIdAsync(student.SessionId);

                // 🔹 Load All Enrollments + Grades + Subjects
                var enrollmentData = await (
                    from e in _uow.EnrollmentRepo.Query()
                    join sub in _uow.SubjectRepository.Query()
                        on e.SubjectId equals sub.SubjectId
                    join g in _uow.GradeRepo.Query()
                        on e.EnrollmentId equals g.EnrollmentId into gradeGroup
                    from g in gradeGroup.DefaultIfEmpty()
                    join sem in _uow.SemesterRepo.Query()
                        on e.SemesterId equals sem.SemesterId
                    where e.StudentId == studentId
                    orderby sem.Order, sub.Title
                    select new
                    {
                        SemesterId = sem.SemesterId,
                        SemesterName = sem.Name,
                        SemesterOrder = sem.Order,
                        SemesterYear = sem.StartDate.Year,

                        SubjectCode = "", // Add Subject.Code here if available
                        SubjectTitle = sub.Title,
                        sub.CreditHours,

                        Grade = g != null ? g.Grad : null,
                        GradePoints = g != null ? g.GradePoints : 0f
                    }
                ).ToListAsync();

                if (!enrollmentData.Any())
                {
                    return ApiResponse<TranscriptDto>.Fail(
                        "No academic record found",
                        ResponseType.NotFound);
                }

                // 🔹 Group By Semester
                var semesterDtos = new List<TranscriptSemesterDto>();

                foreach (var semesterGroup in enrollmentData.GroupBy(x => new
                {
                    x.SemesterId,
                    x.SemesterName,
                    x.SemesterOrder,
                    x.SemesterYear
                })
                         .OrderBy(x => x.Key.SemesterOrder))
                {
                    var subjectDtos = semesterGroup.Select(x => new TranscriptSubjectDto
                    {
                        Title = x.SubjectTitle,
                        CreditHours = x.CreditHours,
                        Grade = x.Grade ?? "Pending",
                        GradePoints = x.Grade != null ? x.GradePoints : 0f
                    }).ToList();

                    // Only passed subjects count as earned credits
                    var earnedCredits = semesterGroup
                        .Where(x => x.Grade != null && x.Grade != "F")
                        .Sum(x => x.CreditHours);

                    var totalCredits = semesterGroup.Sum(x => x.CreditHours);

                    // GPA = weighted average of graded subjects only
                    var gradedSubjects = semesterGroup
                        .Where(x => x.Grade != null)
                        .ToList();

                    double gpa = 0;

                    if (gradedSubjects.Any())
                    {
                        var qualityPoints = gradedSubjects.Sum(x =>
                            x.GradePoints * x.CreditHours);

                        var gradedCreditHours = gradedSubjects.Sum(x =>
                            x.CreditHours);

                        if (gradedCreditHours > 0)
                        {
                            gpa = qualityPoints / gradedCreditHours;
                        }
                    }

                    semesterDtos.Add(new TranscriptSemesterDto
                    {
                        SemesterName = semesterGroup.Key.SemesterName,
                        Season = string.Empty, // Optional: derive from dates if needed
                        Year = semesterGroup.Key.SemesterYear,
                        GPA = Math.Round(gpa, 2),
                        TotalCredits = totalCredits,
                        EarnedCredits = earnedCredits,
                        Subjects = subjectDtos
                    });
                }

                // 🔹 Overall CGPA
                var gradedRecords = enrollmentData
                    .Where(x => x.Grade != null)
                    .ToList();

                double cgpa = 0;
                int totalEarnedCredits = gradedRecords
                    .Where(x => x.Grade != "F")
                    .Sum(x => x.CreditHours);

                int totalRequiredCredits = enrollmentData.Sum(x => x.CreditHours);

                if (gradedRecords.Any())
                {
                    var totalQualityPoints = gradedRecords.Sum(x =>
                        x.GradePoints * x.CreditHours);

                    var totalGradedCredits = gradedRecords.Sum(x =>
                        x.CreditHours);

                    if (totalGradedCredits > 0)
                    {
                        cgpa = totalQualityPoints / totalGradedCredits;
                    }
                }

                // 🔹 Percentage (CGPA out of 4.0)
                var percentage = (cgpa / 4.0) * 100.0;

                // 🔹 Build Final DTO
                var transcript = new TranscriptDto
                {
                    StudentName = student.StudentName,
                    RollNo = student.RollNo,
                    CNIC = student.CNIC,
                    Email = student.StudentEmail,

                    Department = department?.Name ?? string.Empty,
                    Program = department?.Name ?? string.Empty, // BSCS / BSSE etc.
                    Session = session?.Name ?? string.Empty,

                    CGPA = Math.Round(cgpa, 2),
                    TotalEarnedCredits = totalEarnedCredits,
                    TotalRequiredCredits = totalRequiredCredits,
                    Percentage = Math.Round(percentage, 2),

                    Semesters = semesterDtos
                };

                return ApiResponse<TranscriptDto>.Success(
                    transcript,
                    "Transcript generated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<TranscriptDto>.Fail(
                    "Failed to generate transcript",
                    ResponseType.InternalServerError);
            }
        }
    }
}
