using Application_Service.Common.Filters;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Security.Implementation;
using Application_Service.Security.Interface;
using Application_Service.Services.AdminServices.Implementations;
using Application_Service.Services.AdminServices.Interfaces;
using Application_Service.Services.DeptServices.Implementation;
using Application_Service.Services.DeptServices.Interfaces;
using Application_Service.Services.StudentServices.Implementation;
using Application_Service.Services.StudentServices.Interfaces;
using Application_Service.Services.UserManagmentServices.Implementation;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application_Service.DI
{
    public static class ApplicationDIConfigration
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration) => services
            .AddModelValidator()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthenticationServce, AuthenticationServce>()
            .AddScoped<IPasswordEncriptor, PasswordEncriptor>()
            .AddScoped<IAccounService, AccountService>()
            .AddScoped<IStudentService, StudentService>()
            .AddScoped<ISessionService, SessionService>()
            .Configure<JWTSettings>(configuration.GetSection("JwtSettings"))
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<IDepartmentService, DepartmentService>();

    }
}
