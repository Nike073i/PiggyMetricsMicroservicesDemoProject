using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;

namespace Core.Http
{
    public abstract class ServiceClientBase
    {
        private readonly IConsulClient _consulClient;
        protected HttpClient HttpClient { get; }
        private Random _random;

        protected ServiceClientBase(HttpClient httpClient, IConsulClient consulClient)
        {
            _consulClient = consulClient;
            HttpClient = httpClient;
            _random = new Random();
        }

        protected async Task<Uri> GetServiceUriAsync(string serviceName)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulClient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName,
                StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            if (registeredServices == null || !registeredServices.Any())
            {
                throw new Exception($"Consul service: '{serviceName}' was not found.");
            }

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices);

            var uriBuilder = new UriBuilder
            {
                Host = service.Address,
                Port = service.Port
            };

            return uriBuilder.Uri;
        }

        private AgentService GetRandomInstance(IList<AgentService> services)
        {
            return services[_random.Next(0, services.Count)];
        }
    }
}
