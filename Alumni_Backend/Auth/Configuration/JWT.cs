using Alumni_Portal.Auth.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Alumni_Portal.Auth.Configuration
{
    public static class AlumniJwtAuthenticationExtensions
    {
        public static IServiceCollection AddAlumniJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.Configure<JwtSettings>(
                configuration.GetSection("AlumniPortalJwt"));

   
            var jwtSettings = configuration
                .GetSection("AlumniPortalJwt")
                .Get<JwtSettings>();

            if (jwtSettings == null)
                throw new Exception("AlumniPortalJwt section is missing from configuration.");

            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);


            services.AddAuthentication()
                .AddJwtBearer("AlumniBearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            if (ctx.Exception is SecurityTokenExpiredException)
                                ctx.Response.Headers.Append("Token-Expired", "true");
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}