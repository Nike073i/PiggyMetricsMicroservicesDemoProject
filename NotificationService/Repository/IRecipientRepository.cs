using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NotificationService.Domain;

namespace NotificationService.Repository
{
    public interface IRecipientRepository
    {
        /// <summary>
        /// Find recipient by account
        /// </summary>
        /// <param name="accountName">Name of account</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<Recipient> FindAsync(string accountName, CancellationToken token = default);

        /// <summary>
        /// Find for backup
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<List<Recipient>> FindReadyForBackupAsync(CancellationToken token = default);

        /// <summary>
        /// Find for remind
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<List<Recipient>> FindReadyForRemindAsync(CancellationToken token = default);

        /// <summary>
        /// Save recipient
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<Recipient> SaveAsync(Recipient recipient, CancellationToken token = default);
    }
}
