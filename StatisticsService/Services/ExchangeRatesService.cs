using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polly;
using Serilog;
using StatisticsService.Client;
using StatisticsService.Domain;
using StatisticsService.Extensions;

namespace StatisticsService.Services
{
    ///<inheritdoc/>
    public class ExchangeRatesService : IExchangeRatesService
    {
        /// <summary>
        /// Хранилище контейнера курсов валют
        /// </summary>
        private ExchangeRatesContainerStore _containerStore;

        /// <summary>
        /// Клиент для получения курсов валют
        /// </summary>
        private readonly IExchangeRatesClient _exchangeRatesClient;

        private readonly IAsyncPolicy<ExchangeRatesContainer> _policy;

        private readonly ExchangeRatesContainer _fallbackDefault = new ExchangeRatesContainer
        {
            Base = Currency.RUB,
            Date = DateTimeOffset.Now,
            Rates = new Dictionary<string, decimal>()
            {
                { Currency.EUR.ToString(), 0.5M },
                { Currency.RUB.ToString(), 1M },
                { Currency.USD.ToString(), 0.4M }
            }
        };

        public ExchangeRatesService(
            IExchangeRatesClient exchangeRatesClient,
            ExchangeRatesContainerStore containerStore
        )
        {
            _exchangeRatesClient = exchangeRatesClient;
            _containerStore = containerStore;

            _policy = new GetExchangeRatesPolicy(_fallbackDefault).GetRatesPolicy;
        }

        public async Task<decimal> Convert(Currency from, Currency to, decimal amount)
        {
            Dictionary<Currency, decimal> rates = await GetCurrencyRates();
            decimal ratio = Math.Round(rates[to] / rates[from], 4, MidpointRounding.AwayFromZero);
            return amount * ratio;
        }

        public async Task<Dictionary<Currency, decimal>> GetCurrencyRates()
        {
            ExchangeRatesContainer container = await GetRatesContainer(
                CurrencyExtensions.GetDefault()
            );

            return new Dictionary<Currency, decimal>
            {
                { Currency.EUR, container.Rates[Currency.EUR.ToString()] },
                { Currency.USD, container.Rates[Currency.USD.ToString()] },
                { Currency.RUB, 1M }
            };
        }

        public async Task<ExchangeRatesContainer> GetRatesContainer(Currency @base)
        {
            if (!_containerStore.IsActualData)
            {
                await _containerStore.UpdateContainer(
                    _policy.ExecuteAsync(() => _exchangeRatesClient.GetRates(@base))
                );
            }
            return _containerStore.Container;
        }
    }
}
