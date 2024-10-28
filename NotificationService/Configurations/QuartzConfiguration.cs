namespace NotificationService.Configurations
{
    /// <summary>
    /// Quartz configuration
    /// </summary>
    public class QuartzConfiguration
    {
        /// <summary>
        /// Cron period expression for remind
        /// </summary>
        public string RemindCronPeriod { get; set; }

        /// <summary>
        /// Cron period expression for remind
        /// </summary>
        public string BackupCronPeriod { get; set; }
    }
}
