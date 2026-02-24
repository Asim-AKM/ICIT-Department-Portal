using Infrastructure_Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure_Service.DI
{
    public static class InfrastructureDICofigration
    {
        public static IServiceCollection InfrastructureDIConfigur(this IServiceCollection service, IConfiguration configuration) => service

                                                                 .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ICIT_DBConString")));

    }
}
