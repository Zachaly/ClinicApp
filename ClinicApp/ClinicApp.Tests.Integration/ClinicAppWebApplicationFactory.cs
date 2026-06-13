using ClinicApp.Database;
using ClinicApp.Database.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.WebApi.Background;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace ClinicApp.Tests.Integration;

internal class ClinicAppWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ClinicAppWebApplicationFactory(string connectioString) : base()
    {
        _connectionString = connectioString;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using(var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DatabaseUser>>();

            var adminConf = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetSection("DefaultAdmin").Get<CreateUserRequest>();

            var admin = new DatabaseUser
            {
                UserName = adminConf.UserName,
                Email = adminConf.Email,
                LastName = adminConf.LastName,
                FirstName = adminConf.FirstName
            };
            userManager.CreateAsync(admin).GetAwaiter().GetResult();

            userManager.AddPasswordAsync(admin, adminConf.Password).GetAwaiter().GetResult();
            userManager.AddClaimAsync(admin, new Claim(AuthClaimNames.RoleClaim, AuthClaimNames.Admin)).GetAwaiter().GetResult();
        }

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            var dbInitService = services.FirstOrDefault(s => s.ImplementationType == typeof(DatabaseInitializationService));

            if (dbInitService is not null)
            {
                services.Remove(dbInitService);
            }
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SqlServer"] = _connectionString,
            });
        });
    }
}
