using Application_Service.Common;
using Application_Service.DTO_s.DeptDTO_s;

namespace Application_Service.Services.DeptServices.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<GetDepartmentDto>>> GetDepartmentsAsync();
    }
}
