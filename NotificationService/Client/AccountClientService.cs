using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Core.Http;
using Core.Security;
using Microsoft.Extensions.Options;
using NotificationService.Configurations;

namespace NotificationService.Client
{

    /// <inheritdoc/>
    public class AccountClientService: ServiceClientBase, IAccountServiceClient
    {
        private readonly ISecurityTokenStore _securityTokenStore;
        private readonly AccountServiceClientConfiguration _accountServiceClientConfiguration;

        public AccountClientService(HttpClient httpClient,
            IConsulClient consulClient,
            IOptions<AccountServiceClientConfiguration> accountClientOptions,
            ISecurityTokenStore securityTokenStore) 
            : base(httpClient, consulClient)
        {
            _securityTokenStore = securityTokenStore;
            _accountServiceClientConfiguration = accountClientOptions.Value;
        }

        public async Task<string> GetAccountAsync(string accountName, CancellationToken token = default)
        {
            string bearerToken = _securityTokenStore.GetToken(token: token);

            Uri uri = await GetServiceUriAsync(_accountServiceClientConfiguration.Service);

            HttpRequestMessage request = HttpRequestFactory.Get(
                $"{uri.AbsoluteUri.TrimEnd('/')}/accounts/{accountName}",
                bearerToken);

            HttpResponseMessage response = await HttpClient.SendAsync(request, token);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            string responseText = await response.Content.ReadAsStringAsync();
            throw new Exception(responseText);
        }
    }
}