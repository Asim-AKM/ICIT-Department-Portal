using Application_Service.Common;
using Application_Service.RequestAndResponseModel.AuthenticationModels;

namespace Application_Service.Services.UserManagmentServices.Interfaces
{
    public interface IAuthenticationServce
    {
        Task<ApiResponse<string>> UserLogin(UserLoginRequest loginRequest);
    }
}
