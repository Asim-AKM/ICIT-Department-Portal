using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Domain_Service.Enum;

namespace Application_Service.Services.UserManagmentServices.Interfaces
{
    public interface IAccounService
    {
        Task<ApiResponse<CreateUserDto>> CreateAccount(CreateUserDto createUserDto,RoleType role);
    }
}
