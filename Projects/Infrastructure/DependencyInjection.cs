using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Persistence;
using Project.Application.Abstractions;
using Project.Application.Handlers;
using Project.Infrastructure.Repository;
namespace Project.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Projects.Infrastructure")
                )
            );
            services.AddScoped<IProjectDirectory, ProjectDirectory>();
            services.AddScoped<DirectoryHandler>();
            return services;
        }
    }
}
