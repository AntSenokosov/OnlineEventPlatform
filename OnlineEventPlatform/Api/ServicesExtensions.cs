using Application.Catalog.Services;
using Application.Catalog.Services.Interfaces;
using Application.Catalog.Repositories;
using Application.Catalog.Repositories.Interfaces;
using Application.Identity.Repositories;
using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services;
using Application.Identity.Services.Interfaces;
using Application.UserEvents.Repositories;
using Application.UserEvents.Repositories.Interfaces;
using Application.UserEvents.Services;
using Application.UserEvents.Services.Interfaces;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class ServicesExtensions
{
    public static void AddJwt(this IServiceCollection services)
    {
        services.AddOptions();

        var signingKey = new SymmetricSecurityKey("egfwgtw4r32r2te5rty35241234f24ty3413e31qfwreg354yt34t13fw4ege5h34t2fq3vw4g234r23qrw4fgwrw"u8.ToArray());
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var issuer = "Khai";
        var audience = "Department603";

        services.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = issuer;
            options.Audience = audience;
            options.SigningCredentials = signingCredentials;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingCredentials.Key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        var token = context.HttpContext.Request.Headers["Authorization"];
                        if (token.Count > 0 && token[0]!.StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = token[0]!.Substring("Token ".Length).Trim();
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<IPositionRepository, PositionRepository>();
        services.AddTransient<ISpeakerRepository, SpeakerRepository>();
        services.AddTransient<IOnlineEventRepository, OnlineEventRepository>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IPositionService, PositionService>();
        services.AddTransient<ISpeakerService, SpeakerService>();
        services.AddTransient<IOnlineEventService, OnlineEventService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.TryAddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserProfileRepository, UserProfileRepository>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserProfileService, UserProfileService>();

        services.AddTransient<IUserEventRepository, UserEventRepository>();
        services.AddTransient<IUserEventService, UserEventService>();
    }
}