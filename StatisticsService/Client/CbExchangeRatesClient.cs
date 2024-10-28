using Core.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StatisticsService.Configuration;
using StatisticsService.Domain;
using StatisticsService.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StatisticsService.Client
{
    /// <summary>
    /// Клиент курсов Центробанка РФ
    /// </summary>
    public class CbExchangeRatesClient : IExchangeRatesClient
    {
        /// <summary>
        /// Клиент взаимодействия по http
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Настройки клиента для получения курсов валют
        /// </summary>
        private readonly ExchangeRatesClientConfiguration _options;

        public CbExchangeRatesClient(HttpClient httpClient,
            IOptions<ExchangeRatesClientConfiguration> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        ///<inheritdoc/>
        public async Task<ExchangeRatesContainer> GetRates(Currency @base)
        {
            HttpRequestMessage request = HttpRequestFactory.Get($"{_options.BaseUrl.TrimEnd('/')}/latest.js");
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(responseText);
            }

            ExchangeRatesContainer rubExchangeRatesContainer = JsonConvert.DeserializeObject<ExchangeRatesContainer>(responseText);

            return RebuildRates(rubExchangeRatesContainer, @base);
        }

        /// <summary>
        /// Переводим курсы через базовый RUB
        /// </summary>
        private ExchangeRatesContainer RebuildRates(ExchangeRatesContainer rubExchangeRatesContainer, Currency @base)
        {
            if (@base == Currency.RUB)
            {
                return rubExchangeRatesContainer;
            }

            ExchangeRatesContainer baseExchangeRatesContainer = new ExchangeRatesContainer
            {
                Base = @base,
                Date = rubExchangeRatesContainer.Date,
                Rates = new Dictionary<string, decimal>()
            };
            
            if (!rubExchangeRatesContainer.Rates.TryGetValue(@base.GetDescription(), out decimal baseRate))
            {
                throw new Exception($"Unable to find currency rate: {@base.GetDescription()}");
            }

            baseExchangeRatesContainer.Rates.Add(Currency.RUB.GetDescription(), 1 / baseRate);

            foreach (KeyValuePair<string, decimal> rate in rubExchangeRatesContainer.Rates)
            {
                if (rate.Key == @base.GetDescription())
                {
                    continue;
                }

                baseExchangeRatesContainer.Rates.Add(rate.Key, baseRate / rate.Value);
            }

            return baseExchangeRatesContainer;
        }
    }
}
