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
    public class GpaService : IGpaService
    {
        private readonly IUnitOfWork _uow;
        public GpaService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<ApiResponse<CgpaResultDto>> CalculateCgpaAsync(Guid studentId)
        {
            try
            {
                if (studentId == Guid.Empty)
                {
                    return ApiResponse<CgpaResultDto>.Fail(
                        "Invalid student identifier",
                        ResponseType.BadRequest);
                }

                var records = await (
                    from e in _uow.EnrollmentRepo.Query()
                    join g in _uow.GradeRepo.Query()
                        on e.EnrollmentId equals g.EnrollmentId
                    join s in _uow.SubjectRepository.Query()
                        on e.SubjectId equals s.SubjectId
                    where e.StudentId == studentId
                    select new
                    {
                        g.GradePoints,
                        s.CreditHours
                    }
                ).ToListAsync();

                if (!records.Any())
                {
                    return ApiResponse<CgpaResultDto>.Fail(
                        "No graded subjects found",
                        ResponseType.NotFound);
                }

                double totalQualityPoints = records.Sum(x =>
                    x.GradePoints * x.CreditHours);

                int totalCreditHours = records.Sum(x =>
                    x.CreditHours);

                double cgpa = totalQualityPoints / totalCreditHours;

                var result = new CgpaResultDto(
                    studentId,
                    Math.Round(cgpa, 2),
                    totalCreditHours);

                return ApiResponse<CgpaResultDto>.Success(
                    result,
                    "CGPA calculated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<CgpaResultDto>.Fail(
                    "Failed to calculate CGPA",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<GpaResultDto>> CalculateSemesterGpaAsync(Guid studentId,Guid semesterId)
        {
            try
            {
                if (studentId == Guid.Empty || semesterId == Guid.Empty)
                {
                    return ApiResponse<GpaResultDto>.Fail(
                        "Invalid identifiers",
                        ResponseType.BadRequest);
                }

                var records = await (
                    from e in _uow.EnrollmentRepo.Query()
                    join g in _uow.GradeRepo.Query()
                        on e.EnrollmentId equals g.EnrollmentId
                    join s in _uow.SubjectRepository.Query()
                        on e.SubjectId equals s.SubjectId
                    where e.StudentId == studentId
                          && e.SemesterId == semesterId
                    select new
                    {
                        g.GradePoints,
                        s.CreditHours
                    }
                ).ToListAsync();

                if (!records.Any())
                {
                    return ApiResponse<GpaResultDto>.Fail(
                        "No graded subjects found",
                        ResponseType.NotFound);
                }

                double totalQualityPoints = records.Sum(x =>
                    x.GradePoints * x.CreditHours);

                int totalCreditHours = records.Sum(x =>
                    x.CreditHours);

                double gpa = totalQualityPoints / totalCreditHours;

                var result = new GpaResultDto(
                    studentId,
                    semesterId,
                    Math.Round(gpa, 2),
                    totalCreditHours);

                return ApiResponse<GpaResultDto>.Success(
                    result,
                    "Semester GPA calculated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<GpaResultDto>.Fail(
                    "Failed to calculate GPA",
                    ResponseType.InternalServerError);
            }
        }
    }
}
