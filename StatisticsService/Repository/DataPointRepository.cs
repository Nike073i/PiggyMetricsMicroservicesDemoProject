using Core.Configuration;
using Core.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StatisticsService.Domain.Timeseries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatisticsService.Repository
{
    public class DataPointRepository : MongoRepositoryBase<DataPoint>, IDataPointRepository
    {
        public DataPointRepository(IOptions<MongoSettings> options)
            : base(options.Value.CollectionName, options.Value.ConnectionString)
        {
        }

        public async Task<List<DataPoint>> FindByIdAccount(string account)
        {
            IAsyncCursor<DataPoint> result = await Collection.FindAsync(Builders<DataPoint>.Filter.Eq(x => x.Id.Account, account));
            return await result.ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<DataPoint> Save(DataPoint dataPoint)
        {
            ReplaceOneResult result = await Collection.ReplaceOneAsync(Builders<DataPoint>.Filter.Eq(x => x.Id, dataPoint.Id), dataPoint, new ReplaceOptions
            {
                IsUpsert = true
            });

            if (!result.IsAcknowledged)
            {
                throw new Exception("Unable to save datapoint");
            }

            return dataPoint;
        }
    }
}
