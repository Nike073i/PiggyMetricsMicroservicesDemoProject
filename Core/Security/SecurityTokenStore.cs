using Core.Configuration;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Core.Security
{
    public class SecurityTokenStore : ISecurityTokenStore
    {
        /// <summary>
        /// Сервис получения токена
        /// </summary>
        private readonly ISecurityTokenService _securityTokenService;

        private readonly object _lockObject = new object();

        private DateTimeOffset _expiredAt = DateTimeOffset.UtcNow;

        private TokenResponse _token = null;

        private readonly ServiceAuthConfiguration _serviceAuthConfiguration;

        /// <summary>
        /// Контруктор
        /// </summary>
        /// <param name="securityTokenService"></param>
        /// <param name="serviceAuthOptions"></param>
        public SecurityTokenStore(ISecurityTokenService securityTokenService, IOptions<ServiceAuthConfiguration> serviceAuthOptions)
        {
            _securityTokenService = securityTokenService;
            _serviceAuthConfiguration = serviceAuthOptions.Value;
        }


        /// <inheritdoc />
        public string GetToken(bool forceFreshToken = false, CancellationToken token = default)
        {
            if (!HasValidToken(forceFreshToken))
            {
                lock (_lockObject)
                {
                    if (!HasValidToken(forceFreshToken))
                    {
                        RefreshToken(token);
                    }
                }
            }

            return _token?.AccessToken;
        }

        private bool HasValidToken(bool forceFreshToken)
        {
            if (forceFreshToken)
            {
                return false;
            }

            return _token != null && DateTimeOffset.UtcNow.AddMinutes(5) < _expiredAt;
        }

        private void RefreshToken(CancellationToken token = default)
        {
            TokenRequestModel request = new TokenRequestModel
            {
                AuthorityUrl = _serviceAuthConfiguration.AuthorityUrl,
                ClientId = _serviceAuthConfiguration.ClientId,
                ClientSecret = _serviceAuthConfiguration.ClientSecret,
            };

            _token = _securityTokenService.GetTokenAsync(request, token).ConfigureAwait(false).GetAwaiter().GetResult();

            if (_token != null)
            {
                _expiredAt = DateTimeOffset.UtcNow.AddSeconds(_token.ExpiresIn);
            }
        }
    }
}
