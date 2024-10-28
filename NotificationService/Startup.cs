using System;
using Core.Configuration;
using Core.ErrorHandling;
using Core.Extensions;
using Core.RabbitMq;
using Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.AsyncClient;
using NotificationService.Client;
using NotificationService.Configurations;
using NotificationService.Domain;
using NotificationService.Extensions;
using NotificationService.Repository;
using NotificationService.Services;
using Serilog;

namespace NotificationService
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

            services.AddSwaggerAuth(Configuration);

            services.AddBearerAuth(Configuration);
            services.AddSecurityTokenService(Configuration);
            services.AddHttpClient();

            services.AddScoped<IRecipientService, RecipientService>();
            services.Configure<RabbitMqConfiguration>(
                Configuration.GetSection(nameof(RabbitMqConfiguration))
            );
            services.AddSingleton<RabbitMqChannelFactory>();
            services.AddSingleton<NotificationQueuePublisher>(sp =>
            {
                var channelFactory = sp.GetRequiredService<RabbitMqChannelFactory>();
                var topology = new NotificationQueueTopology();
                return new NotificationQueuePublisher(channelFactory, topology);
            });
            services.AddSingleton<SenderService>();
            services.AddScoped<INotificationService, Services.NotificationService>();
            services.AddSingleton<IRecipientRepository, RecipientRepository>();
            services.AddScoped<IEmailContentProvider, EmailContentProvider>();
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));

            services.Configure<RemindEmailConfiguration>(Configuration.GetSection("RemindEmail"));
            services.Configure<BackupEmailConfiguration>(Configuration.GetSection("BackupEmail"));
            services.Configure<AccountServiceClientConfiguration>(
                Configuration.GetSection(nameof(AccountServiceClientConfiguration))
            );

            services.AddConsulClient(Configuration);
            services.AddQuartzNotify(Configuration);

            services
                .AddHttpClient<IAccountServiceClient, AccountClientService>(
                    (sp, client) =>
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.Timeout = TimeSpan.FromSeconds(120);
                    }
                )
                .AddPolly();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Logger.Information("Запуск приложения..");

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId("swagger");
                options.OAuthClientSecret("swagger_auth");
                options.OAuthAppName("Swagger");
                options.OAuthScopes("server", "ui");

                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification service V1");
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