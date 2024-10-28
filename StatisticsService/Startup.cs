using Core.Configuration;
using Core.ErrorHandling;
using Core.Extensions;
using Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Client;
using StatisticsService.Configuration;
using StatisticsService.Repository;
using StatisticsService.Services;
using System;
using Serilog;
using StatisticsService.Extensions;

namespace StatisticsService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddResponseFormatters();

            services.AddHttpClient();

            services.AddSwaggerAuth(Configuration);

            services.AddBearerAuth(Configuration);
            services.AddSecurityTokenService(Configuration);

            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.Configure<ExchangeRatesClientConfiguration>(Configuration.GetSection(nameof(ExchangeRatesClientConfiguration)));

            services.AddHttpContextAccessor();

            services.AddScoped<IDataPointRepository, DataPointRepository>();

            services.AddHttpClient<IExchangeRatesClient, CbExchangeRatesClient>();

            services.AddScoped<IExchangeRatesService, ExchangeRatesService>();
            services.AddScoped<IStatisticsService, Services.StatisticsService>();

            services.AddSingleton<ExchangeRatesContainerStore>();

            services.MongoRegisterClassMaps();

            services.AddConsulClient(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
        {
            Log.Logger.Information("Запуск приложения..");
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId("swagger");
                options.OAuthClientSecret("swagger_auth");
                options.OAuthAppName("Swagger");
                options.OAuthScopes("server", "ui");

                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Account service V1");
            });

            app.UseRouting();

            app.UseDefaultCORSPolicy();
            app.UseBearerAuth();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsulDiscovery();
        }
    }
}
