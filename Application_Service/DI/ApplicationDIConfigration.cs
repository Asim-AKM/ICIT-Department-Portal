using Application_Service.Common.Filters;
using Application_Service.Services.UserManagmentServices.Implementation;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application_Service.DI
{
    public static class ApplicationDIConfigration
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services) => services
            .AddModelValidator()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthenticationServce, AuthenticationServce>()
            .AddScoped<IPasswordEncriptor, PasswordEncriptor>()
            .AddScoped<IAccounService, AccountService>();

    }
}
