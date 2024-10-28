using AccountService.Domain;

namespace AccountService.Extensions
{
    public static class CurrencyExtensions
    {
        public static Currency GetDefault()
        {
            return Currency.USD;
        }
    }
}
