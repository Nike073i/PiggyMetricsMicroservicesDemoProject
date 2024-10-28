using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Client
{
    /// <summary>
    /// Client of account service
    /// </summary>
    public interface IAccountServiceClient
    {
        /// <summary>
        /// Get account
        /// </summary>
        /// <param name="accountName">Account name</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task<string> GetAccountAsync(string accountName, CancellationToken token = default);
    }
}