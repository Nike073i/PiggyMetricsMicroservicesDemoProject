namespace SenderService.Domain
{
    public class MessageToSend
    {
        public string RecipientAddress { get; set; }

        public string AccountName { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string Attachment { get; set; }
    }
}
