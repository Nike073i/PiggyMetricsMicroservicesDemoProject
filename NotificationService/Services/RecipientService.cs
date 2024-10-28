using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NotificationService.Domain;
using NotificationService.Repository;

namespace NotificationService.Services
{
    public class RecipientService : IRecipientService
    {
        private readonly IRecipientRepository _recipientRepository;

        public RecipientService(IRecipientRepository recipientRepository)
        {
            _recipientRepository = recipientRepository;
        }

        public Task<Recipient> FindRecipientAsync(string accountName, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                throw new ArgumentException("Account name is null or empty");
            }

            return _recipientRepository.FindAsync(accountName, token);
        }

        public Task<List<Recipient>> FindReadyToNotifyAsync(NotificationKind kind, CancellationToken token = default)
        {
            switch (kind)
            {
                case NotificationKind.Backup:
                    return _recipientRepository.FindReadyForBackupAsync(token);
                case NotificationKind.Remind:
                    return _recipientRepository.FindReadyForRemindAsync(token);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public async Task<Recipient> SaveAsync(string accountName, Recipient recipient, CancellationToken token = default)
        {
            if(recipient == null)
                throw new ArgumentException("Получатель должен быть задан");

            recipient.AccountName = accountName;
            foreach (var setting in recipient.ScheduledNotifications.Values)
            {
                if (!setting.LastNotified.HasValue)
                {
                    setting.LastNotified = DateTime.UtcNow;
                }
            }

            return await _recipientRepository.SaveAsync(recipient, token);
        }

        public async Task MarkNotifiedAsync(NotificationKind kind, Recipient recipient, CancellationToken token = default)
        {
            if(!recipient.ScheduledNotifications.TryGetValue(kind, out var settings))
            {
                return;
            }

            settings.LastNotified = DateTime.UtcNow;

            await _recipientRepository.SaveAsync(recipient, token);
        }
    }
}