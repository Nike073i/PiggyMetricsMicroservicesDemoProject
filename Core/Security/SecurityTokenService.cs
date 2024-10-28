using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Security
{
    public class SecurityTokenService : ISecurityTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SecurityTokenService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="httpClientFactory">Фарбика http-клиента</param>
        public SecurityTokenService(IHttpClientFactory httpClientFactory, ILogger<SecurityTokenService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequestModel requestModel, CancellationToken token = default)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                DiscoveryDocumentResponse discoveryDocument = null;

                if (requestModel.AuthorityUrl?.StartsWith("http:") == true)
                {
                    DiscoveryDocumentRequest discoverRequest = new DiscoveryDocumentRequest
                    {
                        Address = requestModel.AuthorityUrl,
                        Policy = new DiscoveryPolicy
                        {
                            RequireHttps = false,
                            ValidateIssuerName = false,
                            ValidateEndpoints = false
                        }
                    };

                    discoveryDocument = await client.GetDiscoveryDocumentAsync(discoverRequest, token);
                }
                else
                {
                    discoveryDocument = await client.GetDiscoveryDocumentAsync(requestModel.AuthorityUrl, token);
                }

                if (discoveryDocument.IsError)
                {
                    _logger.LogError($"Ошибка получения токена {discoveryDocument.ErrorType} {discoveryDocument.Error}");
                    return null;
                }

                TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = requestModel.ClientId,
                    ClientSecret = requestModel.ClientSecret
                }, token);

                if (tokenResponse.IsError)
                {
                    _logger.LogError($"Ошибка получения токена {tokenResponse.ErrorType} {tokenResponse.Error}");
                    return null;
                }

                return tokenResponse;
            }
        }
    }
}
