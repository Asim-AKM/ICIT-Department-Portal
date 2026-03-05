using Infrastructure_Service.Data;
using Microsoft.EntityFrameworkCore;

namespace APIGateway_Service.Extentions
{
    /// <summary>
    /// Provides extension methods for applying database migrations in an ASP.NET Core application.
    /// </summary>
    /// <remarks>This class contains methods that help ensure the application's database schema is up to date
    /// by applying any pending migrations at startup. These methods are typically called during application
    /// initialization to automate database updates and reduce manual intervention.</remarks>
    public static class MigrationExtensions
    {
       /// <summary>
       /// Applies any pending migrations to the database associated with the specified application builder to ensure
       /// the schema is up to date.
       /// </summary>
       /// <remarks>This method creates a new service scope, retrieves the application's database context,
       /// and applies any outstanding migrations. It should be called during application startup to keep the database
       /// schema synchronized with the application's data model.</remarks>
       /// <param name="app">The application builder instance used to create a service scope for applying database migrations.</param>
        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
