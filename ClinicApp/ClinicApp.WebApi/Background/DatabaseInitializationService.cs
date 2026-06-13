using ClinicApp.Database;
using ClinicApp.Database.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicApp.WebApi.Background;

public class DatabaseInitializationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        if(!dbContext.UserClaims.Any(c => c.ClaimValue == AuthClaimNames.Admin))
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DatabaseUser>>();

            var adminConf = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetSection("DefaultAdmin").Get<CreateUserRequest>();

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
