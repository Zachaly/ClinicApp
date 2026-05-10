using ClinicApp.Database;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace ClinicApp.WebApi.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder RegisterDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        });

        return builder;
    }

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.UseWolverine(config =>
        {
        });

        return builder;
    }
}
