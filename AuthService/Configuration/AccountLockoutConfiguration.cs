namespace AuthService.Configuration
{
    public class AccountLockoutConfiguration
    {
        public int MaxFailedAttempts { get; set; }
        public int LockoutMinutes { get; set; }
    }
}
