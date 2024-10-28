using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace StatisticsService.Domain.Timeseries
{
    public class DataPoint
    {
        public DataPointId Id { get; set; }

        public List<ItemMetric> Incomes { get; set; }

        public List<ItemMetric> Expenses { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<StatisticMetric, decimal> Statistics { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<Currency, decimal> Rates { get; set; }
    }
}