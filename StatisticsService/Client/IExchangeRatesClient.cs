using StatisticsService.Domain;
using System.Threading.Tasks;

namespace StatisticsService.Client
{
    /// <summary>
    /// Клиент для получения курсов валют
    /// </summary>
    public interface IExchangeRatesClient
    {
        /// <summary>
        /// Получает курсы валют с API
        /// </summary>
        /// <param name="base">Базовый курс</param>
        /// <returns>Контейнер курсов валют</returns>
        Task<ExchangeRatesContainer> GetRates(Currency @base);
    }
}
