using System.ComponentModel;

namespace StatisticsService.Domain
{
    public enum Currency
    {
        [Description("USD")]
        USD = 0,
        [Description("EUR")]
        EUR = 1,
        [Description("RUB")]
        RUB = 2
    }
}
