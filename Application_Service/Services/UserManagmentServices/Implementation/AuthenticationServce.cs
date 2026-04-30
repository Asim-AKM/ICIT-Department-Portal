using Application_Service.Common;
using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.Security.Interface;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;

namespace Application_Service.Services.UserManagmentServices.Implementation
{
    public class AuthenticationServce : IAuthenticationServce
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordEncriptor _passwordEncriptor;
        private readonly IJwtService _jwtService;
        public AuthenticationServce(IUnitOfWork unitOfWork, IPasswordEncriptor passwordEncriptor,IJwtService jwtService)
        {
            _uow = unitOfWork;
            _passwordEncriptor = passwordEncriptor;
            _jwtService = jwtService;
        }
        public async Task<ApiResponse<string>> UserLogin(UserLoginRequest request)
        {
            var user = await _uow.UserRepo.GetByIdentifier(request.CNIC);

            if (user is null)
                return ApiResponse<string>.Fail("Invalid credentials", ResponseType.Unauthorized);

            if (user.Status is UserStatus.Inactive or UserStatus.Blocked)
                return ApiResponse<string>.Fail("Account is not active", ResponseType.Unauthorized);

            var credential = await _uow.UserCreadentialRepo
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            if (credential is null)
                return ApiResponse<string>.Fail("Invalid credentials", ResponseType.Unauthorized);

            var isValidPassword = await _passwordEncriptor.VerifyPassword(
                request.Password,
                credential.PasswordSalt,
                credential.PasswordHash);

            if (!isValidPassword)
                return ApiResponse<string>.Fail("Invalid credentials", ResponseType.Unauthorized);

            var roles = await _uow.UserRoleRepo.GetUserRoleByUserId(user.UserId);

            var token = await _jwtService.GenerateJwtToken(user, roles);

            return ApiResponse<string>.Success(
                "Login successful",
                token,
                ResponseType.Ok);
        }
    }
}
