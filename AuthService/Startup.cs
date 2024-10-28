using System;
using AuthService.Configuration;
using AuthService.Domain;
using AuthService.Extensions;
using AuthService.Services;
using AuthService.Utils;
using Core.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AuthService
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
            services.AddControllersWithViews().AddResponseFormatters();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "default",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(origin => true);
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                    }
                );
            });

            services.Configure<PepperConfiguration>(
                Configuration.GetSection(nameof(PepperConfiguration))
            );
            services.Configure<MongoDbRepositoryConfiguration>(
                Configuration.GetSection(nameof(MongoDbRepositoryConfiguration))
            );
            services.Configure<AccountLockoutConfiguration>(
                Configuration.GetSection(nameof(AccountLockoutConfiguration))
            );

            services
                .AddIdentityServer(options =>
                {
                    options.Discovery.CustomEntries.Add("current_user", "~/users/current");
                    options.Discovery.CustomEntries.Add("create_user", "~/users");
                })
                .AddMongoRepository()
                .AddProfileService()
                .AddClients()
                .AddIdentityApiResources()
                .AddPersistedGrants()
                .AddDeveloperSigningCredential()
                .AddUsers();

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPasswordHasher<User>, ArgonPasswordHasher>();
            services.AddTransient<
                IResourceOwnerPasswordValidator,
                ResourceOwnerPasswordValidator
            >();

            services.AddTransient<IEndpointRouter, EndpointRouter>();

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("server", policy => policy.RequireClaim("scope", "server"));
            });

            services.MongoIgnoreExtraElements();
            services.ConfigureCookieOptions();
            services.AddConsulClient(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
        {
            Log.Logger.Information("Запуск приложения..");
            app.UseCors("default");

            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsulDiscovery();
        }
    }
}
