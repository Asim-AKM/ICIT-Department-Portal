using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.RequestAndResponseModel.Pagination;
using Domain_Service.Enum;

namespace Application_Service.Services.UserManagmentServices.Interfaces
{
    public interface IUserService 
    {
        Task<ApiResponse<PaginationResponse<GetUserDto>>> GetAllUsers(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<PaginationResponse<GetUserDto>>> GetUsersByFilter(GetUserByRoleAndStatusRequest request);
        Task<ApiResponse<string>> UpdateUser(UpdateUserDto request);
    }
}
