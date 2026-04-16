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
namespace Alumni_Portal.Entity_Directories
{
    public static class DependencyInjection
    {
        
            public static IServiceCollection AddEntityDirectoriesInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {
            services.AddDbContext<IndividualDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")

            ));

            services.AddDbContext<ProjectDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            ));

            services.AddDbContext<PostDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            ));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<PostService>();
            services.AddScoped<SharedRepository>();

            return services;
            }
        }
    }

