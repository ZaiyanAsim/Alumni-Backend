using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.OpenPortalPages.MainPage.Respositories;
using Alumni_Portal.OpenPortalPages.MainPage.Services;
using Alumni_Portal.OpenPortalPages.ProjectListing.Repository;
using Alumni_Portal.OpenPortalPages.ProjectListing.Services;
using Entity_Directories.Repositories;
using Entity_Directories.Services;
using Entity_Directories.Services.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;




namespace Alumni_Portal.OpenPortalPages.ProjectFeed
{


    public static class DependencyInjection
    {

        public static IServiceCollection AddProjectFeedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped<ProjectFeedService>();
            services.AddScoped<ProjectFeedRepository>();


            return services;
        }
    }
}


