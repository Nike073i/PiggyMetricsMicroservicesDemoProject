using StatisticsService.Domain;

namespace StatisticsService.Extensions
{
    public static class CurrencyExtensions
    {
        public static Currency GetDefault()
        {
            return Currency.USD;
        }
    }
}
