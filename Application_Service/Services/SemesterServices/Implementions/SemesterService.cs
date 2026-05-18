using Application_Service.Common;
using Application_Service.DTO_s.SemesterDTO_s;
using Application_Service.Services.SemesterServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SemesterServices.Implementions
{
    public class SemesterService : ISemesterService
    {
        IUnitOfWork _unitOfWork;
        public SemesterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ApiResponse<GetSemeterDto>> GetSemesters()
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse<List<GetSemeterDto>>> GetSemestersAsync(Guid sessionId)
        {
            // Optional: verify that the session exists
            var session = await _unitOfWork.SessionRepo.GetByIdAsync(sessionId);
            if (session == null)
            {
                return ApiResponse<List<GetSemeterDto>>.Fail(
                    "Session not found.",
                    ResponseType.NotFound
                );
            }

            // Get all semesters for the specified session and order them
            var semesters = await _unitOfWork.SemesterRepo
                .Query()
                .Where(s => s.SessionId == sessionId)
                .OrderBy(s => s.Order)
                .ToListAsync();

            // If no semesters are found
            if (semesters == null || !semesters.Any())
            {
                return ApiResponse<List<GetSemeterDto>>.Fail(
                    new List<GetSemeterDto>(),
                    "No semesters found for this session.",
                    ResponseType.NotFound
                );
            }
            // Map entities to DTOs
            var result = semesters.Select(s => new GetSemeterDto
            {
                SemesterId = s.SemesterId,
                SessionId = s.SessionId,
                Name = s.Name,
                Order = s.Order,
                AcademicYear = s.AcademicYear,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            // Return successful response
            return ApiResponse<List<GetSemeterDto>>.Success(
                result,
                "Semesters retrieved successfully.",
                ResponseType.Ok
            );
        }
    }
}
