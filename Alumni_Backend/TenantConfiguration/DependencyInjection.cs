using Alumni_Portal.TenantConfiguration.Parameter;
using Microsoft.EntityFrameworkCore;

namespace Alumni_Portal.TenantConfiguration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTenantConfigurationInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                ));
            services.AddScoped<ProjectParameters>();
            services.AddScoped<IndividualParameters>();
            services.AddScoped<PostParameters>();
            services.AddScoped<ConfigService>();
            return services;
        }
    }
}
