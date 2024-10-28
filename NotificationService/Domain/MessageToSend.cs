using System;

namespace NotificationService.Domain
{
    public class MessageToSend
    {
        public MessageToSend(EmailContent content, Recipient recipient, string attachment)
        {
            RecipientAddress = recipient.Email;
            Subject = content.Subject;
            Text = content.Text;
            Attachment = attachment;
            AccountName = recipient.AccountName;
        }

        public string RecipientAddress { get; set; }

        public string AccountName { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string Attachment { get; set; }
    }
}
