using dev.codingWombat.Vombatidae.store;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStoreServiceConfig(this IServiceCollection services)
        {
            services.TryAddTransient<ICacheRepository, CacheRepository>();

            return services;
        }
    }
}