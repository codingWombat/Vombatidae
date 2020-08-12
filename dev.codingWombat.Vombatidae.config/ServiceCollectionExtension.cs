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
            services.Configure<CorsConfiguration>(configuration.GetSection(CorsConfiguration.Configuration));
            services.Configure<VortexLoggingConfig>(configuration.GetSection(VortexLoggingConfig.Configuration));

            return services;
        }
    }
}