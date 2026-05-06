using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.DeptRepo;
using Domain_Service.RepoInterfaces.EmailRepo;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Infrastructure_Service.Persistance.Repositories.AdminRepo_s;
using Infrastructure_Service.Persistance.Repositories.DeptRepo_s;
using Infrastructure_Service.Persistance.Repositories.EmailRepo_s;
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

        // ✅ DbContext should be registered ONCE
        .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ICIT_DBConString"))) 

        // ✅ Register generic repository
        .AddScoped(typeof(IRepository<>), typeof(Repository<>))

        // ✅ Register UnitOfWork
        .AddScoped<IUnitOfWork, UnitOfWork>()


        // ✅ Only keep these if they are NOT created by UnitOfWork
        .AddScoped<IEmailRepository, EmailRepository>();




    }
}