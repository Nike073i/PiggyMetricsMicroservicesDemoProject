using StatisticsService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatisticsService.Services
{
    /// <summary>
    /// Сервис конвертации валют
    /// </summary>
    public interface IExchangeRatesService
    {
        /// <summary>
        /// Получить курсы валют
        /// </summary>
        /// <returns>Курсы валют</returns>
        Task<Dictionary<Currency, decimal>> GetCurrencyRates();

        /// <summary>
        /// Возвращает курс валюты относительно переданной в параметрах
        /// </summary>
        /// <param name="from">Исходный курс</param>
        /// <param name="to">Конечный курс</param>
        /// <param name="amount">Сумма</param>
        /// <returns>Курс валюты</returns>
        Task<decimal> Convert(Currency from, Currency to, decimal amount);

        /// <summary>
        /// Получить курсы по базовой валюте
        /// </summary>
        /// <param name="base">Базовая валюта</param>
        /// <returns>Курсы валют</returns>
        Task<ExchangeRatesContainer> GetRatesContainer(Currency @base);
    }
}
