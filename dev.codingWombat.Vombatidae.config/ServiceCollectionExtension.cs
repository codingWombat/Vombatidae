using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dev.codingWombat.Vombatidae.config
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigurationServiceConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<CacheConfiguration>(configuration.GetSection(CacheConfiguration.Configuration));

            return services;
        }
    }
}