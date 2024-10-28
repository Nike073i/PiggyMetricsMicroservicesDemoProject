using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using StatisticsService.Domain.Timeseries;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StatisticsService.Mongo.Serializers
{
    [BsonSerializer(typeof(DataPointIdSerializer))]
    public class DataPointIdSerializer : IBsonSerializer<DataPointId>
    {
        private readonly Regex dataPointIdRegex = new Regex(@"^(DataPointId)({(account=)'(.*)', (date=)(.*)})");

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Guid.Parse(BsonSerializer.Deserialize<string>(context.Reader));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DataPointId value)
        {
            BsonSerializer.Serialize(context.Writer, value.ToString());
        }

        DataPointId IBsonSerializer<DataPointId>.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            string dataPointIdString = BsonSerializer.Deserialize<string>(context.Reader);
            Match dataPointIdMatch = dataPointIdRegex.Match(dataPointIdString);

            if (!dataPointIdMatch.Success)
            {
                throw new Exception($"Unable to perform serialize of dataPointId {dataPointIdString}");
            }

            return new DataPointId(dataPointIdMatch.Groups[4].Value, DateTimeOffset.ParseExact(dataPointIdMatch.Groups[6].Value, "ddMMyyyyHHmmss", CultureInfo.InvariantCulture));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            BsonSerializer.Serialize(context.Writer, value.ToString());
        }

        public Type ValueType
        {
            get { return typeof(DataPointId); }
        }
    }
}
