using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// ServiceCollectionExtension
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Jwt Service 
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        byte[] keyBytes = Encoding.ASCII.GetBytes(configuration["jwt:secret"]);
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        return services;
    }
}