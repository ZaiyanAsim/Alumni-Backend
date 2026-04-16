using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Abstractions;
using Users.Application.Handlers;
using Users.Infrastructure.Persistance;
using Users.Infrastructure.Repository;
namespace Users.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Users.Infrastructure")
                )
            );
            services.AddScoped<IUserDirectory, UserDirectory>();
            services.AddScoped<DirectoryHandler>();

            return services;
        }
    }
}
