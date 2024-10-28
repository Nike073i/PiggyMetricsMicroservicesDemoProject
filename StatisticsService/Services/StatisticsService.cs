using Serilog;
using StatisticsService.Domain;
using StatisticsService.Domain.Timeseries;
using StatisticsService.Extensions;
using StatisticsService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsService.Services
{
    ///<inheritdoc/>
    public class StatisticsService : IStatisticsService
    {
        /// <summary>
        /// Репозиторий статистики
        /// </summary>
        private IDataPointRepository _repository;

        /// <summary>
        /// Сервис конвертации валют
        /// </summary>
        private IExchangeRatesService _ratesService;

        public StatisticsService(IDataPointRepository repository, IExchangeRatesService ratesService)
        {
            _repository = repository;
            _ratesService = ratesService;
        }

        public async Task<List<DataPoint>> FindByAccountName(string accountName)
        {
            if (accountName == null)
            {
                throw new ArgumentException("Account name is null", nameof(accountName));
            }

            return await _repository.FindByIdAccount(accountName);
        }

        public async Task<DataPoint> Save(string accountName, Account account)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            DateTimeOffset startOfToday = new DateTimeOffset(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, TimeSpan.Zero);

            DataPointId pointId = new DataPointId(accountName, startOfToday);

            List<ItemMetric> incomes = account.Incomes
                    .Select(async x => await CreateItemMetric(x))
                    .Select(x => x.Result)
                    .ToList();

            List<ItemMetric> expenses = account.Expenses
                    .Select(async x => await CreateItemMetric(x))
                    .Select(x => x.Result)
                    .ToList();

            Dictionary<StatisticMetric, decimal> statistics = await CreateStatisticMetrics(incomes, expenses, account.Saving);

            DataPoint dataPoint = new DataPoint
            {
                Id = pointId,
                Incomes = incomes,
                Expenses = expenses,
                Statistics = statistics,
                Rates = await _ratesService.GetCurrencyRates()
            };

            Log.Debug("new datapoint has been created: {}", pointId);

            return await _repository.Save(dataPoint);
        }

        private async Task<Dictionary<StatisticMetric, decimal>> CreateStatisticMetrics(List<ItemMetric> incomes, List<ItemMetric> expenses, Saving saving)
        {
            decimal savingAmount = await _ratesService.Convert(saving.Currency, CurrencyExtensions.GetDefault(), saving.Amount);

            decimal expensesAmount = expenses
                     .Select(x => x.Amount)
                     .Sum();

            decimal incomesAmount = incomes
                     .Select(x => x.Amount)
                     .Sum();

            return new Dictionary<StatisticMetric, decimal>
            {
                { StatisticMetric.EXPENSES_AMOUNT, expensesAmount },
                { StatisticMetric.INCOMES_AMOUNT, incomesAmount },
                { StatisticMetric.SAVING_AMOUNT, savingAmount }
            };
        }

        private async Task<ItemMetric> CreateItemMetric(Item item)
        {
            decimal baseAmount = await _ratesService.Convert(item.Currency, CurrencyExtensions.GetDefault(), item.Amount);
            decimal amount = Math.Round(baseAmount / item.Period.GetBaseRatio(), 4, MidpointRounding.AwayFromZero);
            return new ItemMetric(item.Title, amount);
        }
    }
}
