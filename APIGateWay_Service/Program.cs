
using APIGateway_Service.Extentions;
using Infrastructure_Service.DI;

namespace APIGateway_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerConfigration();

            builder.Services.InfrastructureDIConfigur(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.AddMiddlewareConfigration();
            app.Run();
        }
    }
}
