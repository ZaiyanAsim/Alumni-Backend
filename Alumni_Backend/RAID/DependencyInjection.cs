using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Alumni_Portal.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Alumni_Portal.Infrastructure.Persistence;
using Entity_Directories.Services.Abstractions;
using Entity_Directories.Repositories;
using Entity_Directories.Services;
using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.RAID.Services;
using Alumni_Portal.RAID.Login;
namespace Alumni_Portal.RAID
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddRAIDInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddScoped<EmailService>();
            services.AddScoped<LoginService>();
            

            return services;
        }
    }
}


