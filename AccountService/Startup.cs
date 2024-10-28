using System;
using AccountService.Client;
using AccountService.Configuration;
using AccountService.Data.Contexts;
using AccountService.Repository;
using AccountService.Services;
using Core.Configuration;
using Core.ErrorHandling;
using Core.Extensions;
using Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace AccountService
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
            services.AddControllers().AddResponseFormatters();

            services.AddHttpClient();

            services.AddSwaggerAuth(Configuration);

            services.AddBearerAuth(Configuration);
            services.AddSecurityTokenService(Configuration);

            services.Configure<PostgreSqlSettings>(
                Configuration.GetSection(nameof(PostgreSqlSettings))
            );
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.Configure<AuthServiceClientConfiguration>(
                Configuration.GetSection(nameof(AuthServiceClientConfiguration))
            );
            services.Configure<StatisticsClientConfiguration>(
                Configuration.GetSection(nameof(StatisticsClientConfiguration))
            );

            services.AddHttpContextAccessor();

            services.AddScoped<IAccountRepository, EfAccountRepository>();
            services.AddScoped<IAccountService, Services.AccountService>();

            services
                .AddHttpClient<IAuthServiceClient, AuthServiceClient>(
                    (sp, client) =>
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.Timeout = TimeSpan.FromSeconds(120);
                    }
                )
                .AddPolly();

            services
                .AddHttpClient<IStatisticsServiceClient, StatisticsServiceClient>(
                    (sp, client) =>
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.Timeout = TimeSpan.FromSeconds(120);
                    }
                )
                .AddPolly();

            services.AddConsulClient(Configuration);

            services.AddDbContext<AccountDbContext>(
                (serviceProvider, dbBuilder) =>
                {
                    var postgreSqlSettings = serviceProvider
                        .GetRequiredService<IOptions<PostgreSqlSettings>>()
                        .Value;
                    dbBuilder.UseNpgsql(postgreSqlSettings.ConnectionString);
                }
            );
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
