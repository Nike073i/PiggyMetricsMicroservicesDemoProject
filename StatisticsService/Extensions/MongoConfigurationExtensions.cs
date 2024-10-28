using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using StatisticsService.Domain.Timeseries;
using StatisticsService.Mongo.Serializers;

namespace StatisticsService.Extensions
{
    public static class MongoConfigurationExtensions
    {
        /// <summary>
        /// Расширенный маппинг классов для MongoDB
        /// </summary>
        /// <param name="serviceCollection">Коллекция сервисов</param>
        /// <returns></returns>
        public static IServiceCollection MongoRegisterClassMaps(this IServiceCollection serviceCollection)
        {
            BsonClassMap.RegisterClassMap<DataPoint>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(x => x.Id).SetSerializer(new DataPointIdSerializer());
            });

            return serviceCollection;
        }
    }
}
