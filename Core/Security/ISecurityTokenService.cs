using IdentityModel.Client;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Security
{
    /// <summary>
    /// Клиент для получения токена
    /// </summary>
    public interface ISecurityTokenService
    {
        /// <summary>
        /// Получение токена авторизации
        /// </summary>
        /// <param name="requestModel">Запрос</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        Task<TokenResponse> GetTokenAsync(TokenRequestModel requestModel, CancellationToken token = default);
    }
}
