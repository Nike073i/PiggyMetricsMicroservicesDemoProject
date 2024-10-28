namespace NotificationService.Domain
{
    public class EmailContent
    {
        /// <summary>
        /// Subject letter
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Text letter
        /// </summary>
        public string Text { get; set;}

        /// <summary>
        /// Attacment letter
        /// </summary>
        public string Attachment { get; set; }
    }
}
