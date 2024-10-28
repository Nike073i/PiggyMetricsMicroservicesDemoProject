using App.Metrics;
using App.Metrics.Counter;

namespace NotificationService.Domain
{
    /// <summary>
    /// Бизнес метрики
    /// </summary>
    public static class BusinessMetrics
    {
        /// <summary>
        /// Метрика количества отправленных писем
        /// </summary>
        public static CounterOptions EmailSentCount => new CounterOptions
        {
            Context = "Notification",
            Name = "EmailSentCount",
            MeasurementUnit = Unit.Calls,
            Tags = new MetricTags("NotificationKey","NotificationService")
        };
    }
}
