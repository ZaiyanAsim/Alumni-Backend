using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.OpenPortalPages.MainPage.Respositories;
using Alumni_Portal.OpenPortalPages.MainPage.Services;
using Alumni_Portal.OpenPortalPages.ProjectListing.Repository;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services;
using Alumni_Portal.Profiles.Repositories;
using Alumni_Portal.Profiles.Services;
using Entity_Directories.Repositories;
using Entity_Directories.Services;
using Entity_Directories.Services.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;




namespace Alumni_Portal.Profiles
{


    public static class DependencyInjection
    {

        public static IServiceCollection AddProfileInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ProjectProfileReadService>();
            services.AddScoped<ProjectReadRepo>();


            services.AddScoped<ProjectProfileUpdateService>();
            services.AddScoped<ProjectUpdateRepo>();


            return services;
        }
    }
}

