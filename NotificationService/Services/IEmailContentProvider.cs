using NotificationService.Domain;

namespace NotificationService.Services
{
    /// <summary>
    /// Content provider fore Email
    /// </summary>
    public interface IEmailContentProvider
    {
        /// <summary>
        /// Get email content
        /// </summary>
        /// <param name="kind">Notification kind</param>
        /// <returns></returns>
        EmailContent GetContent(NotificationKind kind);
    }
}
