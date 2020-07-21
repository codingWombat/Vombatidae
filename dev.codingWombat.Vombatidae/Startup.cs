using System;
using dev.codingWombat.Vombatidae.config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dev.codingWombat.Vombatidae
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCoreServiceConfig();
            services.AddConfigurationServiceConfig(Configuration);
            
            var cacheConfig = new CacheConfiguration();
            Configuration.GetSection(CacheConfiguration.Configuration).Bind(cacheConfig);

            if (cacheConfig.UseReddis)
            {
                if (string.IsNullOrWhiteSpace(cacheConfig.Host) || string.IsNullOrWhiteSpace(cacheConfig.Instance))
                {
                    throw new Exception("Reddis not configured properly");
                }

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheConfig.Host;
                    options.InstanceName = cacheConfig.Instance;
                }); 
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}