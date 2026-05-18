using Application_Service.Common;
using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Application_Service.Services.GradeServices.Interfaces;
using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.GradeServices.Implementations
{
    public class GradeService : IGradeService
    {

        private readonly IUnitOfWork _uow;
        public GradeService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<ApiResponse<string>> CreateOrUpdateGradeAsync(CreateOrUpdateGradeRequest request)
        {
            try
            {
                if (request.EnrollmentId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid enrollment id",
                        ResponseType.BadRequest);
                }

                // 🔹 Check Enrollment exists
                var enrollment = await _uow.EnrollmentRepo.GetByIdAsync(request.EnrollmentId);

                if (enrollment == null)
                {
                    return ApiResponse<string>.Fail(
                        "Enrollment not found",
                        ResponseType.NotFound);
                }

                //var isLocked = await _resultLockService.IsResultLockedAsync(subject.DepartmentId, subject.SemesterId);

                //if (isLocked)
                //{
                //    return ApiResponse<string>.Fail(
                //        "Results are locked. Grades cannot be modified.",
                //        ResponseType.BadRequest);
                //}

                // 🔹 Calculate grade
                var (grade, gradePoints, total) = CalculateGrade(
                    request.MidtermMarks,
                    request.FinalMarks,
                    request.AssignmentMarks,
                    request.QuizMarks);

                // 🔹 Check if grade already exists
                var existingGrade = await _uow.GradeRepo.Query()
                    .FirstOrDefaultAsync(g => g.EnrollmentId == request.EnrollmentId);

                if (existingGrade != null)
                {
                    // UPDATE
                    existingGrade.MidtermMarks = request.MidtermMarks;
                    existingGrade.FinalMarks = request.FinalMarks;
                    existingGrade.AssignmentMarks = request.AssignmentMarks;
                    existingGrade.QuizMarks = request.QuizMarks;
                    existingGrade.TotalMarks = total;
                    existingGrade.Grad = grade;
                    existingGrade.GradePoints = gradePoints;

                    await _uow.GradeRepo.Update(existingGrade);
                }
                else
                {
                    // CREATE
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
                        GradePoints = gradePoints
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
                    "Failed to save grade",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<Grade>> GetGradeByEnrollmentAsync(Guid enrollmentId)
        {
            try
            {
                var grade = await _uow.GradeRepo.Query()
                    .FirstOrDefaultAsync(g => g.EnrollmentId == enrollmentId);

                if (grade == null)
                {
                    return ApiResponse<Grade>.Fail(
                        "Grade not found",
                        ResponseType.NotFound);
                }

                return ApiResponse<Grade>.Success(
                    grade,
                    "Grade retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<Grade>.Fail(
                    "Failed to get grade",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<List<Grade>>> GetStudentTranscriptAsync(Guid studentId)
        {
            try
            {
                var grades = await _uow.GradeRepo.Query()
                    .Where(g => g.Enrollment.StudentId == studentId)
                    .ToListAsync();

                if (!grades.Any())
                {
                    return ApiResponse<List<Grade>>.Success(
                        new List<Grade>(),
                        "No grades found",
                        ResponseType.Ok);
                }

                return ApiResponse<List<Grade>>.Success(
                    grades,
                    "Transcript retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<Grade>>.Fail(
                    "Failed to get transcript",
                    ResponseType.InternalServerError);
            }
        }

        private (string grade, float gradePoints, int totalMarks) CalculateGrade(int mid, int final, int assignment, int quiz)
        {
            var total = mid + final + assignment + quiz;

            if (total >= 85) return ("A", 4.0f, total);
            if (total >= 80) return ("A-", 3.7f, total);
            if (total >= 75) return ("B+", 3.3f, total);
            if (total >= 70) return ("B", 3.0f, total);
            if (total >= 65) return ("B-", 2.7f, total);
            if (total >= 60) return ("C+", 2.3f, total);
            if (total >= 55) return ("C", 2.0f, total);
            if (total >= 50) return ("D", 1.0f, total);

            return ("F", 0.0f, total);
        }
    }
}
