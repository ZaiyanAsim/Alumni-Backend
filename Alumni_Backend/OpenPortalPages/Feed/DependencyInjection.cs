using Alumni_Portal.Entity_Directories.Repositories;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.OpenPortalPages.MainPage.Respositories;
using Alumni_Portal.OpenPortalPages.MainPage.Services;
using Entity_Directories.Repositories;
using Entity_Directories.Services;
using Entity_Directories.Services.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;




namespace Alumni_Portal.OpenPortalPages.Feed
{
    
    
        public static class DependencyInjection
        {

            public static IServiceCollection AddFeedInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {
                

                services.AddScoped<FeedService>();
                services.AddScoped<FeedPageRepository>();
                

                return services;
            }
        }
    }



