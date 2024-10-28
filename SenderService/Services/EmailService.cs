using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SenderService.Configs;
using SenderService.Domain;
using Serilog;

namespace SenderService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceSettings _settings;
        private readonly IMetrics _metrics;

        public EmailService(IOptions<EmailServiceSettings> settings, IMetrics metrics)
        {
            _settings = settings.Value;
            _metrics = metrics;
        }

        public async Task SendAsync(MessageToSend letter, CancellationToken token = default)
        {
            var subject = letter.Subject;
            var text = string.Format(letter.Text, letter.AccountName);

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("", _settings.Username));
            message.To.Add(MailboxAddress.Parse(letter.RecipientAddress));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = text };

            if (!string.IsNullOrEmpty(letter.Attachment))
            {
                var part = new MimePart("application/json", "json")
                {
                    Content = new MimeContent(
                        new MemoryStream(Encoding.UTF8.GetBytes(letter.Attachment))
                    ),
                    FileName = "attachment"
                };

                builder.Attachments.Add(part);
            }

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _settings.Host,
                    _settings.Port,
                    SecureSocketOptions.StartTls,
                    token
                );
                await client.AuthenticateAsync(_settings.Username, _settings.Password, token);
                await client.SendAsync(message, token);

                await client.DisconnectAsync(true, token);
            }

            Log.Logger.Information(
                $"'{letter.Subject}' email notification has been send to '{letter.RecipientAddress}'"
            );

            _metrics.Measure.Counter.Increment(BusinessMetrics.EmailSentCount);
        }
    }
}
