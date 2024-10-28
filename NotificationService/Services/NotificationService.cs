using System;
using System.Threading;
using System.Threading.Tasks;
using NotificationService.Client;
using NotificationService.Domain;
using Serilog;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRecipientService _recipientService;
        private readonly SenderService _senderService;
        private readonly IEmailContentProvider _contentProvider;
        private readonly IAccountServiceClient _accountClient;

        public NotificationService(
            IRecipientService recipientService,
            SenderService senderService,
            IEmailContentProvider contentProvider,
            IAccountServiceClient accountClient
        )
        {
            _recipientService = recipientService;
            _senderService = senderService;
            _contentProvider = contentProvider;
            _accountClient = accountClient;
        }

        public async Task SendBackupNotifications(CancellationToken token = default)
        {
            var type = NotificationKind.Backup;

            var recipients = await _recipientService.FindReadyToNotifyAsync(type, token);
            Log.Logger.Information(
                $"found '{recipients.Count}' recipients for backup notification"
            );

            EmailContent content = _contentProvider.GetContent(type);

            foreach (var recipient in recipients)
            {
                try
                {
                    string attachment = await _accountClient.GetAccountAsync(
                        recipient.AccountName,
                        token
                    );

                    var messageToSend = new MessageToSend(content, recipient, attachment);

                    _senderService.Send(messageToSend);
                    await _recipientService.MarkNotifiedAsync(type, recipient, token);
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"an error during backup notification for {recipient}", e);
                }
            }
        }

        public async Task SendRemindNotifications(CancellationToken token = default)
        {
            var type = NotificationKind.Remind;
            var recipients = await _recipientService.FindReadyToNotifyAsync(type, token);
            Log.Logger.Information($"found {recipients.Count} recipients for remind notification");

            EmailContent content = _contentProvider.GetContent(type);

            foreach (var recipient in recipients)
            {
                try
                {
                    var messageToSend = new MessageToSend(content, recipient, null);
                    _senderService.Send(messageToSend);
                    await _recipientService.MarkNotifiedAsync(type, recipient, token);
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"an error during remind notification for {recipient}", e);
                }
            }
        }
    }
}