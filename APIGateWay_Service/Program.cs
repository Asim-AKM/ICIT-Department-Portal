
using APIGateway_Service.Extentions;
using Application_Service.DI;
using Infrastructure_Service.DI;
using Microsoft.Extensions.FileProviders;

namespace APIGateway_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerConfigration();
            builder.Services.AddJwtValidation(builder.Configuration);

            builder.Services.AddApplicationDI(builder.Configuration);
            builder.Services.InfrastructureDIConfigur(builder.Configuration);


            var app = builder.Build();


            // Automatically apply pending migrations
            app.ApplyMigrations();

            app.UseCors("AllowAngular");
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"uploads")),
                RequestPath = "/uploads"
            });
            // Configure the HTTP request pipeline.
            app.AddMiddlewareConfigration();
            app.Run();
        }
    }
}
