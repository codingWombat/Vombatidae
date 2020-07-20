using dev.codingWombat.Vombatidae.core;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreServiceConfig(this IServiceCollection services)
        {
            services.TryAddTransient<IBurrowCreator, BurrowCreator>();
            services.TryAddTransient<IBurrowReader, BurrowReader>();
            services.TryAddTransient<IBurrowUpdater, BurrowUpdater>();
            services.TryAddTransient<ICacheRepository, CacheRepository>();
            return services;
        }
    }
}