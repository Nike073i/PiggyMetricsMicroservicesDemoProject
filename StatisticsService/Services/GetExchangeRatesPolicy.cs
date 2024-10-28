using System;
using Polly;
using Serilog;
using StatisticsService.Domain;

namespace StatisticsService.Services
{
    public class GetExchangeRatesPolicy
    {
        public IAsyncPolicy<ExchangeRatesContainer> GetRatesPolicy { get; }

        public GetExchangeRatesPolicy(ExchangeRatesContainer FallBackValue)
        {
            var exponentialRetryPolicy = Policy
                .Handle<Exception>(ex =>
                {
                    Log.Error($"An exception has occured: {ex.Message}. Retrying");
                    return true;
                })
                .WaitAndRetryAsync(3, times => TimeSpan.FromMilliseconds(100 * Math.Pow(2, times)));

            GetRatesPolicy = Policy<ExchangeRatesContainer>
                .Handle<Exception>(ex =>
                {
                    Log.Error($"An exception has occured: {ex.Message}. Returning fallback value.");
                    return true;
                })
                .FallbackAsync(FallBackValue)
                .WrapAsync(exponentialRetryPolicy);
        }
    }
}
