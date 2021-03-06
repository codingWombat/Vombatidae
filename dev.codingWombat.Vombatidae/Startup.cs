using System;
using CodingWombat.Incub8Vortex.Logger;
using dev.codingWombat.Vombatidae.config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dev.codingWombat.Vombatidae
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool UseCors { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient<IControllerHelper, ControllerHelper>();
            services.AddControllers();
            services.AddCoreServiceConfig();
            services.AddStoreServiceConfig();
            services.AddConfigurationServiceConfig(Configuration);
            services.AddHostedService<HistoryService>();

            var corsConfig = new CorsConfiguration();
            Configuration.GetSection(CorsConfiguration.Configuration).Bind(corsConfig);

            if (corsConfig.Origins.Length != 0)
            {
                UseCors = true;
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder => builder.WithOrigins(corsConfig.Origins).AllowAnyMethod().AllowAnyHeader());
                });    
            }
            
            var cacheConfig = new CacheConfiguration();
            Configuration.GetSection(CacheConfiguration.Configuration).Bind(cacheConfig);

            if (cacheConfig.UseRedis)
            {
                if (string.IsNullOrWhiteSpace(cacheConfig.Host) || string.IsNullOrWhiteSpace(cacheConfig.Instance))
                {
                    throw new Exception("Redis not configured properly");
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            var loggerConfig = new VortexLoggingConfig();
            Configuration.GetSection(VortexLoggingConfig.Configuration).Bind(loggerConfig);

            if (loggerConfig.Enable)
            {
                var vortex = new VortexLoggerConfiguration();
                Configuration.GetSection("Logging").GetSection("Vortex").Bind(vortex);
                loggerFactory.AddVortexLogger(vortex);
            }
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            if (UseCors)
            {
                app.UseCors();
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}