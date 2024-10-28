using StatisticsService.Domain.Timeseries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatisticsService.Repository
{
    public interface IDataPointRepository
    {
        Task<List<DataPoint>> FindByIdAccount(string account);

        Task<DataPoint> Save(DataPoint dataPoint);
    }
}
