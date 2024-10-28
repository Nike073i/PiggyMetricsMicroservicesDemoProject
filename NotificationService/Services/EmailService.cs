using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Domain;
using Serilog;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMetrics _metrics;
        private readonly EmailServiceSettings _settings;

        public EmailService(IOptions<EmailServiceSettings> settings, IMetrics metrics)
        {
            _metrics = metrics;
            _settings = settings.Value;
        }
        
        public async Task SendAsync(EmailContent emailContent, Recipient recipient,
            string attachment, CancellationToken token = default)
        {
            var subject = emailContent.Subject;
            var text = string.Format(emailContent.Text, recipient.AccountName);

            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress("", _settings.Email));
            message.To.Add(MailboxAddress.Parse(recipient.Email));
            message.Subject = subject;

            var builder = new BodyBuilder ();

            builder.HtmlBody  =  text;

            if (!string.IsNullOrEmpty(attachment))
            {
                var part = new MimePart("application/json","json")
                {
                    Content = new MimeContent(new MemoryStream(Encoding.UTF8.GetBytes(attachment))),
                    FileName = emailContent.Attachment
                };

                builder.Attachments.Add(part);
            }

            message.Body = builder.ToMessageBody();
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.Smtp, _settings.Port, SecureSocketOptions.Auto, token);
                await client.AuthenticateAsync(_settings.Email, _settings.Password, token);
                await client.SendAsync(message, token);
 
                await client.DisconnectAsync(true, token);
            }
            
            Log.Logger.Information($"'{emailContent.Subject}' email notification has been send to '{recipient.Email}'");
           
            _metrics.Measure.Counter.Increment(BusinessMetrics.EmailSentCount);
        }
    }
}