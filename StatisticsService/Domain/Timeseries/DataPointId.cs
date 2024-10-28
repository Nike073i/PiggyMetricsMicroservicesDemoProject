using System;

namespace StatisticsService.Domain.Timeseries
{
    public class DataPointId
    {
        public string Account { get; }

        public DateTimeOffset Date { get; }

        public DataPointId(string account, DateTimeOffset date)
        {
            Account = account;
            Date = date;
        }

        public override string ToString()
        {
            return $"DataPointId{{account='{Account}', date={Date:ddMMyyyyHHmmss}}}";
        }
    }
}