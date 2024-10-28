using Core.Security;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthService.Client
{
    /// <summary>
    /// Клиент сервиса авторизации
    /// </summary>
    public class AuthServiceClient
    {
        /// <summary>
        /// Клиент взаимодействия по http
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Хранилище токена авторизации
        /// </summary>
        private readonly ISecurityTokenStore _securityTokenStore;

        public AuthServiceClient(HttpClient httpClient,
            ISecurityTokenStore securityTokenStore)
        {
            _httpClient = httpClient;
            _securityTokenStore = securityTokenStore;
        }

        ///<inheritdoc/>
        public async Task CreateUserAsync(Contracts.Order order, CancellationToken token = default)
        {
            string authToken = _securityTokenStore.GetToken(token: token);

            HttpResponseMessage response = await HttpRequestFactory.Post(_httpClientFactory,
                requestUri: $"{_settings.Host}{_settings.ApiEndpoints.CreateOrder}",
                value: order,
                bearerToken: authToken,
                extraHeaders: GetExtraHeaders(),
                token: token);

            if (!response.IsSuccessStatusCode)
                throw new TaskControlApiException(response.ContentAsString());

            return response.ContentAsType<Contracts.Order>();
        }
    }
}
