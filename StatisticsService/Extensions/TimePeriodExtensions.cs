using StatisticsService.Domain;
using System.Collections.Generic;

namespace StatisticsService.Extensions
{
    public static class TimePeriodExtensions
    {
        private static readonly Dictionary<TimePeriod, decimal> _baseRatio = new Dictionary<TimePeriod, decimal>
        {
            { TimePeriod.YEAR, 365.2425M },
            { TimePeriod.QUARTER, 91.3106M },
            { TimePeriod.MONTH, 30.4368M },
            { TimePeriod.DAY, 1M },
            { TimePeriod.HOUR, 0.0416M }
        };

        public static decimal GetBaseRatio(this TimePeriod timePeriod)
        {
            return _baseRatio[timePeriod];
        }
    }
}
