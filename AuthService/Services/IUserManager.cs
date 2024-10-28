using AuthService.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Services
{
    /// <summary>
    /// Сервис управления пользователями
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Создает пользователя
        /// </summary>
        /// <param name="userModel">Пользователь для создания</param>
        /// <param name="token">Токен отмены</param>
        Task CreateAsync(UserModel userModel, CancellationToken token = default);
    }
}
