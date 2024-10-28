
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Send backup notification
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task SendBackupNotifications(CancellationToken token = default);
        
        /// <summary>
        /// Send remind notification
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task SendRemindNotifications(CancellationToken token = default);
    }
}