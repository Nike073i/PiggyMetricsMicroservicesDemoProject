using System;

namespace StatisticsService.Domain.Timeseries
{
    public class ItemMetric
    {
        public string Title;

        public decimal Amount { get; set; }

        public ItemMetric(string title, decimal amount)
        {
            Title = title;
            Amount = amount;
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }

            if (o == null || GetType() != o.GetType())
            {
                return false;
            }

            ItemMetric that = (ItemMetric)o;

            return Title.Equals(that.Title, StringComparison.InvariantCultureIgnoreCase);

        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}