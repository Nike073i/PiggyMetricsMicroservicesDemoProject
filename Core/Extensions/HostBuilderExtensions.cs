using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Добавить метрики prometheus
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UsePrometheusMetrics(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .UseMetricsWebTracking(opt =>
               {
                   opt.OAuth2TrackingEnabled = true;
               })
                .UseMetricsEndpoints(opt =>
                {
                    opt.EnvironmentInfoEndpointEnabled = true;
                    opt.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    opt.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                });
        }
    }
}
