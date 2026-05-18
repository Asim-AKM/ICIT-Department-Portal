using Application_Service.Common;
using Application_Service.DTO_s.SemesterDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using Application_Service.Services.SemesterServices.Interfaces;
using Application_Service.Services.StudentServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.SemesterServices.Implementions
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGpaService _gpaService;
        public PromotionService(IUnitOfWork unitOfWork,IGpaService gpaService)
        {
            _uow = unitOfWork;
            _gpaService = gpaService;
        }
        public async Task<ApiResponse<PromotionResultDto>> SingleStudentSemesterPromotionAsync(PromotionRequest request)
        {
            try
            {
                // 🔹 VALIDATION
                if (request.UserId == Guid.Empty || request.SemesterId == Guid.Empty || request.DepartmentId == Guid.Empty)
                {
                    return ApiResponse<PromotionResultDto>.Fail(
                        "Invalid request parameters",
                        ResponseType.BadRequest);
                }

                // 🔹 GET STUDENT
                var studentEntity = await _uow.StudentRepo.GetStudentByUserId(request.UserId);

                if (studentEntity == null)
                {
                    return ApiResponse<PromotionResultDto>.Fail(
                        "Student not found",
                        ResponseType.NotFound);
                }

                // 🔹 DEPARTMENT CHECK
                if (studentEntity.DepartmentId != request.DepartmentId)
                {
                    return ApiResponse<PromotionResultDto>.Fail(
                        "Student does not belong to this department",
                        ResponseType.BadRequest);
                }

                var studentId = studentEntity.StudentId;

                // 🔹 GPA CALCULATION
                var gpaResult = await _gpaService.CalculateSemesterGpaAsync(studentId, request.SemesterId);

                if (!gpaResult.IsSuccess)
                {
                    return ApiResponse<PromotionResultDto>.Fail(
                        "GPA calculation failed",
                        ResponseType.BadRequest);
                }

                var gpa = gpaResult.Data.GPA;

                // 🔹 CGPA CALCULATION
                var cgpaResult = await _gpaService.CalculateCgpaAsync(studentId);

                var cgpa = cgpaResult.Data.CGPA;

                // 🔹 FAILED SUBJECTS
                var failedSubjects = await (
                    from e in _uow.EnrollmentRepo.Query()
                    join g in _uow.GradeRepo.Query()
                        on e.EnrollmentId equals g.EnrollmentId
                    where e.StudentId == studentId
                          && e.SemesterId == request.SemesterId
                          && g.Grad == "F"
                    select e
                ).CountAsync();

                // 🔹 GET SEMESTER ORDER
                var currentSemester = await _uow.SemesterRepo.Query()
                    .FirstOrDefaultAsync(s => s.SemesterId == request.SemesterId);

                if (currentSemester == null)
                {
                    return ApiResponse<PromotionResultDto>.Fail(
                        "Semester not found",
                        ResponseType.NotFound);
                }

                var nextSemester = await _uow.SemesterRepo.Query()
                    .FirstOrDefaultAsync(s => s.Order == currentSemester.Order + 1);

                // 🔥 RULE ENGINE
                bool isPromoted = true;
                bool isProbation = false;
                string status;

                if (gpa < 2.0)
                {
                    isPromoted = false;
                    status = "Repeat Semester";
                }
                else if (gpa < 2.5)
                {
                    isProbation = true;
                    status = "Promoted with Probation";

                    if (nextSemester != null)
                        studentEntity.SamesterId = nextSemester.SemesterId;
                }
                else
                {
                    status = "Promoted";

                    if (nextSemester != null)
                        studentEntity.SamesterId = nextSemester.SemesterId;
                }

                // 🔹 UPDATE STUDENT
                if (isPromoted)
                {
                    await _uow.StudentRepo.Update(studentEntity);
                }

                await _uow.SaveChangesAsync();

                // 🔹 RESPONSE
                return ApiResponse<PromotionResultDto>.Success(
                    new PromotionResultDto
                    {
                        StudentId = studentId,
                        CurrentSemesterId = request.SemesterId,
                        NextSemesterId = isPromoted ? nextSemester?.SemesterId : null,
                        GPA = gpa,
                        CGPA = cgpa,
                        FailedSubjects = failedSubjects,
                        IsPromoted = isPromoted,
                        IsOnProbation = isProbation,
                        Status = status
                    },
                    "Promotion evaluated successfully",
                    ResponseType.Ok);
            }
            catch (Exception ex)
            {
                return ApiResponse<PromotionResultDto>.Fail(
                    $"Promotion failed: {ex.Message}",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<List<PromotionResultDto>>> FullBatchSemesterPromotionAsync(BatchPromotionRequest request)
        {
            try
            {
                // 🔹 VALIDATION
                if (request.SemesterId == Guid.Empty || request.DepartmentId == Guid.Empty)
                {
                    return ApiResponse<List<PromotionResultDto>>.Fail(
                        "Invalid request parameters",
                        ResponseType.BadRequest);
                }

                // 🔹 GET STUDENTS (FILTERED)
                var students = await _uow.StudentRepo.Query()
                    .Where(s => s.SamesterId == request.SemesterId
                             && s.DepartmentId == request.DepartmentId)
                    .ToListAsync();

                if (!students.Any())
                {
                    return ApiResponse<List<PromotionResultDto>>.Fail(
                        "No students found for this department and semester",
                        ResponseType.NotFound);
                }

                var results = new List<PromotionResultDto>();

                // 🔥 PROCESS EACH STUDENT
                foreach (var student in students)
                {
                    try
                    {
                        PromotionRequest promotionRequest = new PromotionRequest
                        {
                            UserId = student.UserId,
                            SemesterId = request.SemesterId,
                            DepartmentId = request.DepartmentId
                        };
                        var result = await SingleStudentSemesterPromotionAsync(promotionRequest);

                        if (result.IsSuccess && result.Data != null)
                        {
                            results.Add(result.Data);
                        }
                    }
                    catch
                    {
                        // continue batch even if one fails
                        continue;
                    }
                }

                await _uow.SaveChangesAsync();

                return ApiResponse<List<PromotionResultDto>>.Success(
                    results,
                    "Batch promotion completed successfully",
                    ResponseType.Ok);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PromotionResultDto>>.Fail(
                    $"Batch promotion failed: {ex.Message}",
                    ResponseType.InternalServerError);
            }
        }
    }
}
