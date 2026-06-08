using ClinicApp.Database;
using ClinicApp.Database.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicApp.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    public static async Task AddDefaultAdmin(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if(!dbContext.UserClaims.Any(c => c.ClaimValue == AuthClaimNames.Admin))
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DatabaseUser>>();
            var adminConf = app.Configuration.GetSection("DefaultAdmin").Get<CreateUserRequest>();

            var admin = new DatabaseUser
            {
                UserName = adminConf.UserName,
                Email = adminConf.Email,
                LastName = adminConf.LastName,
                FirstName = adminConf.FirstName
            };
            await userManager.CreateAsync(admin);
            await userManager.AddPasswordAsync(admin, adminConf.Password);
            await userManager.AddClaimAsync(admin, new Claim(AuthClaimNames.RoleClaim, AuthClaimNames.Admin));
        }

        
    }
}
