using ClinicApp.Database;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
