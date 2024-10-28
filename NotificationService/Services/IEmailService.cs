using System.Threading;
using System.Threading.Tasks;
using NotificationService.Domain;

namespace NotificationService.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailContent">Email content</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="attachment">Attachment</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        Task SendAsync(EmailContent emailContent, Recipient recipient,
            string attachment, CancellationToken token = default);
    }
}