using System;
using System.Diagnostics.CodeAnalysis;

namespace NotificationService.Domain
{
    public class NotificationSettings
    {
        [NotNull]
        public bool? IsActive { get; set; }
        
        [NotNull]
        public Frequency? Frequency { get; set; }
        public DateTime? LastNotified { get; set; }
    }
}