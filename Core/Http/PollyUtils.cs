using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace Core.Http
{
    public static class PollyUtils
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy( int count)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(count, attempt =>
                        TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)),
                    (ex, duration) =>
                    {
                        Log.Logger.Error(
                            $"Error on send query to service. Retry through '{duration.TotalMilliseconds}' ms. '{ex.Result?.StatusCode}':'{ex.Exception?.Message}' ");
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds)
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(seconds);
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
            int eventsAllowedBeforeBreak,
            double durationOfBreakSec)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(eventsAllowedBeforeBreak,
                    TimeSpan.FromSeconds(durationOfBreakSec),
                    (ex, breakDelay) => { Log.Logger.Error($"CircuitBreaker '{breakDelay.TotalMilliseconds}' StatusCode'{ex.Result?.StatusCode}':{ex.Exception?.Message}"); },
                    () =>
                    {
                        Log.Logger.Information(".Breaker logging: Call ok! Closed the circuit again!");
                    },
                    () =>
                    {
                        Log.Logger.Information(".Breaker logging: Half-open: Next call is a trial!");
                    }
                );
        }
    }
}
