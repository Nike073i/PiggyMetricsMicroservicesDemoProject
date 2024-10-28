using System;
using App.Metrics;
using App.Metrics.Counter;

namespace SenderService.Domain
{
    public static class BusinessMetrics
    {
        public static CounterOptions EmailSentCount =>
            new CounterOptions
            {
                Context = "Notification",
                Name = "EmailSentCount",
                MeasurementUnit = Unit.Calls,
                Tags = new MetricTags("NotificationKey", "NotificationService")
            };
    }
}
