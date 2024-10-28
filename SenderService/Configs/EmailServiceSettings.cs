namespace SenderService.Configs
{
    public class EmailServiceSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
