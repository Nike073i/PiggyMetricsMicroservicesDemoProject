using AccountService.Configuration;
using AccountService.Domain;
using Consul;
using Core.Http;
using Core.Security;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AccountService.Client
{
    public class StatisticsServiceClient : ServiceClientBase, IStatisticsServiceClient
    {
        /// <summary>
        /// Хранилище токена авторизации
        /// </summary>
        private readonly ISecurityTokenStore _securityTokenStore;

        /// <summary>
        /// Конфигурация сервиса статистики
        /// </summary>
        private readonly StatisticsClientConfiguration _configuration;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="httpClient">Клиент взаимодействия по http</param>
        /// <param name="consulClient">Клиент для работы с Consul</param>
        /// <param name="configuration">Конфигурация сервиса статистики</param>
        /// <param name="securityTokenStore">Хранилище токена авторизации</param>
        public StatisticsServiceClient(HttpClient httpClient,
            IConsulClient consulClient,
            IOptions<StatisticsClientConfiguration> configuration,
            ISecurityTokenStore securityTokenStore)
            : base(httpClient, consulClient)
        {
            _configuration = configuration.Value;
            _securityTokenStore = securityTokenStore;
        }

        public async Task UpdateStatistics(string accountName, Account account, CancellationToken token = default)
        {
            string bearerToken = _securityTokenStore.GetToken(token: token);

            Uri uri = await GetServiceUriAsync(_configuration.Service);

            HttpRequestMessage request = HttpRequestFactory.Put($"{uri.AbsoluteUri.TrimEnd('/')}/statistics/{accountName}", account, bearerToken);

            HttpResponseMessage response = await HttpClient.SendAsync(request, token);

            if (!response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                throw new Exception(responseText);
            }
        }
    }
}
