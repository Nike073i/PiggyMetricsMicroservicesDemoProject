using Core.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Добавить Polly
        /// </summary>
        /// <param name="builder">Строитель запросов HTTP</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddPolly(this IHttpClientBuilder builder)
        {
            return  builder.AddPolicyHandler(PollyUtils.GetRetryPolicy(count: 2))
                .AddPolicyHandler(PollyUtils.GetTimeoutPolicy(seconds: 15))
                .AddPolicyHandler(PollyUtils.GetCircuitBreakerPolicy( 
                    eventsAllowedBeforeBreak: 3,
                    durationOfBreakSec: 60));
        }
    }
}
