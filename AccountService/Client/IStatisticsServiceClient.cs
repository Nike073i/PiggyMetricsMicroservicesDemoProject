using AccountService.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace AccountService.Client
{
    public interface IStatisticsServiceClient
    {
        Task UpdateStatistics(string accountName, Account account, CancellationToken token = default);
    }
}
