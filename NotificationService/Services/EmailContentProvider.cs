using System;
using Microsoft.Extensions.Options;
using NotificationService.Configurations;
using NotificationService.Domain;

namespace NotificationService.Services
{
    public class EmailContentProvider: IEmailContentProvider
    {
        private readonly IOptions<BackupEmailConfiguration> _backupOptions;
        private readonly IOptions<RemindEmailConfiguration> _remindOptions;

        public EmailContentProvider(IOptions<BackupEmailConfiguration> backupOptions,
            IOptions<RemindEmailConfiguration> remindOptions)
        {
            _backupOptions = backupOptions;
            _remindOptions = remindOptions;
        }

        public EmailContent GetContent(NotificationKind kind)
        {
            switch(kind)
            {
                case NotificationKind.Backup:
                    return _backupOptions.Value;
                case NotificationKind.Remind:
                    return _remindOptions.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}
