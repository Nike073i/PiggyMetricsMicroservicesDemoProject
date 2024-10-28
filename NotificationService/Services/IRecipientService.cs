using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NotificationService.Domain;

namespace NotificationService.Services
{
    public interface IRecipientService
    {
        /// <summary>
        /// Find recipient async
        /// </summary>
        /// <param name="accountName">Account</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<Recipient> FindRecipientAsync(string accountName, CancellationToken token = default);

        /// <summary>
        /// Find ready to notify recipients
        /// </summary>
        /// <param name="kind">Notification kind</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<List<Recipient>> FindReadyToNotifyAsync(NotificationKind kind, CancellationToken token = default);

        /// <summary>
        /// Save recipient
        /// </summary>
        /// <param name="accountName">Asoount name</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<Recipient> SaveAsync(string accountName, Recipient recipient, CancellationToken token = default);

        /// <summary>
        /// Mark recipient as notified
        /// </summary>
        /// <param name="kind">Notification kind</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task MarkNotifiedAsync(NotificationKind kind, Recipient recipient, CancellationToken token = default);
    }
}