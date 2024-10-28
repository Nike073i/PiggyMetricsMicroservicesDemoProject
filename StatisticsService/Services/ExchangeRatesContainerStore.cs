using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using StatisticsService.Domain;

namespace StatisticsService.Services
{
    public class ExchangeRatesContainerStore
    {
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);

        public ExchangeRatesContainer Container { get; private set; }

        public bool IsActualData => Container != null;

        public async Task UpdateContainer(Task<ExchangeRatesContainer> supplier)
        {
            await _mutex.WaitAsync();
            try
            {
                if (!IsActualData)
                {
                    Container = await supplier;
                }
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}
