using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;

namespace Application_Service.Services.StudentServices.Interfaces
{
    public interface IGpaService 
    {
        Task<ApiResponse<GpaResultDto>> CalculateSemesterGpaAsync(
       Guid studentId,
       Guid semesterId);

        Task<ApiResponse<CgpaResultDto>> CalculateCgpaAsync(
            Guid studentId);
    }
}
