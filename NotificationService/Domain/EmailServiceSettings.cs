namespace NotificationService.Domain
{
    public class EmailServiceSettings
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}