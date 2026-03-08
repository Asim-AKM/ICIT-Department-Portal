
using APIGateway_Service.Extentions;
using Application_Service.DI;
using Infrastructure_Service.DI;

namespace APIGateway_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowICITDeparmentUI", policy =>
                {
                    policy
                        .WithOrigins("https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerConfigration();

            builder.Services.AddApplicationDI();
            builder.Services.InfrastructureDIConfigur(builder.Configuration);


            var app = builder.Build();

            // Automatically apply pending migrations
            app.ApplyMigrations();

            // Configure the HTTP request pipeline.
            app.AddMiddlewareConfigration();
            app.UseCors("AllowICITDeparmentUI");
            app.Run();
        }
    }
}
