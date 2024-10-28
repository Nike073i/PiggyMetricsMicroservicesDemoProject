using Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Core.Security
{
    public static class SecurityServiceExtensions
    {
        /// <summary>
        /// Регистрация сервиса получения токенов для межсервисного взаимодействия
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <param name="configuration">Конфигурация</param>
        public static void AddSecurityTokenService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceAuthConfiguration>(configuration.GetSection(nameof(ServiceAuthConfiguration)));
            services.AddSingleton<ISecurityTokenService, SecurityTokenService>();
            services.AddSingleton<ISecurityTokenStore, SecurityTokenStore>();
        }

        private static BearerAuthConfiguration GetAuthConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(BearerAuthConfiguration)).Get<BearerAuthConfiguration>();
        }

        public static IServiceCollection AddBearerAuth(this IServiceCollection services, string authorityUrl, string resourceName)
        {
            if (string.IsNullOrWhiteSpace(authorityUrl) || string.IsNullOrWhiteSpace(resourceName))
            {
                throw new Exception("Не удалось определить URL сервиса авторизации");
            }

            if (!Uri.IsWellFormedUriString(authorityUrl, UriKind.RelativeOrAbsolute))
            {
                throw new Exception("URL сервиса авторизации имеет неверный формат");
            }

            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authorityUrl;
                    options.RequireHttpsMetadata = false;

                    options.Audience = resourceName;
                });

            return services;
        }

        public static IServiceCollection AddBearerAuth(this IServiceCollection services, BearerAuthConfiguration config)
        {
            if (config == null)
            {
                throw new Exception("Не удалось определить конфигурацию сервиса авторизации");
            }

            services.AddBearerAuth(config.AuthorityUrl, config.ResourceName);

            return services;
        }

        public static IServiceCollection AddBearerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection configurationSection = configuration.GetSection(nameof(BearerAuthConfiguration));
            BearerAuthConfiguration config = configurationSection.Get<BearerAuthConfiguration>();

            services.AddBearerAuth(config);

            services.Configure<BearerAuthConfiguration>(configurationSection);

            return services;
        }

        public static IServiceCollection AddDefaultCORSPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }

        public static void AddSwaggerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection configurationSection = configuration.GetSection(nameof(BearerAuthConfiguration));
            BearerAuthConfiguration config = configurationSection.Get<BearerAuthConfiguration>();

            services.AddSwaggerAuth(config?.SwaggerAuthUrl);
        }

        public static void AddSwaggerAuth(this IServiceCollection services, string authorityOrigin)
        {
            if (string.IsNullOrWhiteSpace(authorityOrigin))
                throw new Exception("Не удалось определить хост сервиса авторизации для swagger");

            if (!authorityOrigin.StartsWith("http") || !Uri.IsWellFormedUriString(authorityOrigin, UriKind.Absolute))
                throw new Exception("Неверный формат хоста сервиса авторизации");

            var authority = authorityOrigin.TrimEnd('/');

            services.AddSwaggerGen(x =>
            {
                x.OperationFilter<AuthorizeCheckOperationFilter>();

                x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "bearer",

                    OpenIdConnectUrl = new Uri($"{authority}/.well-known/openid-configuration"),

                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "ui", "ui" },
                                { "server", "server" }
                            }
                        }
                    }
                });
            });
        }
    }
}
