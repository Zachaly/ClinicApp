using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Application.User.Validation;
using ClinicApp.Database;
using ClinicApp.Database.Model;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Service;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.Infrastructure.Configuration;
using ClinicApp.Infrastructure.Repository;
using ClinicApp.Infrastructure.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

        return builder;
    }

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.UseWolverine(config =>
        {
            config.Discovery.IncludeAssembly(typeof(LoginHandler).Assembly);
        });

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("Authorization"));

        builder.Services.AddIdentity<DatabaseUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            var authConfig = builder.Configuration.GetSection("Authorization").Get<AuthConfig>();

            var keyBytes = Encoding.UTF8.GetBytes(authConfig.SecretKey);
            var key = new SymmetricSecurityKey(keyBytes);

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidIssuer = authConfig.Issuer,
                ValidAudience = authConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
            };

            opt.TokenValidationParameters = validationParameters;
            opt.MapInboundClaims = false;
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicyNames.RequireAdmin,
                pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Admin));
            
            options.AddPolicy(AuthPolicyNames.RequireDoctor,
                pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Admin, AuthClaimNames.Doctor));

            options.AddPolicy(AuthPolicyNames.RequireReceptionist,
                pol => pol.RequireClaim(AuthClaimNames.RoleClaim, AuthClaimNames.Receptionist, AuthClaimNames.Admin));
        });

        return builder;
    }
}
