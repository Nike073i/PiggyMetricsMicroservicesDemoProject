using System.Threading;

namespace Core.Security
{
    /// <summary>
    /// Хранилище токена авторизации
    /// </summary>
    public interface ISecurityTokenStore
    {
        /// <summary>
        /// Получает токен для клиента из конфигурации
        /// </summary>
        /// <param name="forceFreshToken">Получить обновленный токен</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        string GetToken(bool forceFreshToken = false, CancellationToken token = default);
    }
}
