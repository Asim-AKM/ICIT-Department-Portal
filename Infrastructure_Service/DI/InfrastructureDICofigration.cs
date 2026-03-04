using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Infrastructure_Service.Persistance.Repositories.AdminRepo_s;
using Infrastructure_Service.Persistance.Repositories.StudentRepo_s;
using Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s;
using Infrastructure_Service.Persistance.UniteOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure_Service.DI
{
    public static class InfrastructureDICofigration
    {
        public static IServiceCollection InfrastructureDIConfigur(this IServiceCollection service, IConfiguration configuration) => service

            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ICIT_DBConString")))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IUserRepo, UserRepo>()
            .AddScoped<IUserRoleRepo, UserRoleRepo>()
            .AddScoped<IUserCreadentialRepo, UserCreadentialRepo>()
            .AddScoped<IStudentRepo, StudentRepo>()
            .AddScoped<ISessionRepo, SessionRepo>();


    }
}
