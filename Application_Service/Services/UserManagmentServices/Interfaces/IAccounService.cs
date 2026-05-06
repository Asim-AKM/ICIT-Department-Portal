using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Domain_Service.Enum;
using Microsoft.AspNetCore.Http;

namespace Application_Service.Services.UserManagmentServices.Interfaces
{
    public interface IAccounService
    {
        Task<ApiResponse<CreateUserResponseDto>> CreateAccount(CreateUserDto request, RoleType role);
        Task<ApiResponse<GetUserProfileDto>> GetProfileDetails(Guid userId);
        Task<ApiResponse<string>> UploadProfileImage(UserProfileImageUploadDto request);
        Task<ApiResponse<string>> RemoveProfileImage(Guid userId);

    }
}
