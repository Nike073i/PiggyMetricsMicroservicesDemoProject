using System;
using Consul;
using Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Extensions
{
	public static class ServiceDiscoveryExtension
	{
		public static IServiceCollection AddConsulClient(this IServiceCollection services, IConfiguration configuration)
		{
            services.Configure<ConsulConfiguration>(configuration.GetSection(nameof(ConsulConfiguration)));
            services.AddSingleton<IConsulClient, ConsulClient>(sp => new ConsulClient(consulConfig =>
            {
                IOptions<ConsulConfiguration> consulConfiguration = sp.GetRequiredService<IOptions<ConsulConfiguration>>();
                consulConfig.Address = new Uri(consulConfiguration.Value.Uri);
            }, null, handlerOverride =>
            { 
                handlerOverride.Proxy = null;
                handlerOverride.UseProxy = false;
            }));

			return services;
		}

		public static IApplicationBuilder UseConsulDiscovery(this IApplicationBuilder app)
		{
			var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            var discoveryConfig = app.ApplicationServices
                .GetRequiredService<IOptions<ConsulConfiguration>>()
                .Value?.Discovery;

			if(discoveryConfig == null)
				throw new ArgumentException($"{nameof(DiscoveryConfiguration)} Не задана конфигурация");

			var registration = new AgentServiceRegistration()
			{
				ID = discoveryConfig.ServiceId,
				Name = discoveryConfig.Service,
				Address = discoveryConfig.Address,
				Port = discoveryConfig.Port
			};

            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
			{
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
			});

			return app;
		}
	}
}
