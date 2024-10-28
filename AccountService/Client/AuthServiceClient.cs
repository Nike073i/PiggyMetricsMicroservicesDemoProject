using AccountService.Configuration;
using AccountService.Domain;
using Core.Http;
using Core.Security;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace AccountService.Client
{
    public class AuthServiceClient : ServiceClientBase, IAuthServiceClient
    {
        /// <summary>
        /// Хранилище токена авторизации
        /// </summary>
        private readonly ISecurityTokenStore _securityTokenStore;
        private readonly AuthServiceClientConfiguration _configuration;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="httpClient">Клиент взаимодействия по http</param>
        /// <param name="securityTokenStore">Хранилище токена авторизации</param>
        /// <param name="consulClient">Клиент для работы с Consul</param>
        public AuthServiceClient(HttpClient httpClient,
            ISecurityTokenStore securityTokenStore,
            IOptions<AuthServiceClientConfiguration> configuration,
            IConsulClient consulClient)
            :base(httpClient, consulClient)
        {
            _configuration = configuration.Value;
            _securityTokenStore = securityTokenStore;
        }

        ///<inheritdoc/>
        public async Task CreateUserAsync(User user, CancellationToken token = default)
        {
            string bearerToken = _securityTokenStore.GetToken(token: token);

            Uri uri = await GetServiceUriAsync(_configuration.Service);

            HttpRequestMessage request = HttpRequestFactory.Post($"{uri.AbsoluteUri.TrimEnd('/')}/users/", user, bearerToken);

            HttpResponseMessage response = await HttpClient.SendAsync(request, token);

            if (!response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                throw new Exception(responseText);
            }
        }
    }
}
