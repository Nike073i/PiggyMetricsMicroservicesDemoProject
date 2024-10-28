using AccountService.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace AccountService.Client
{
    public interface IAuthServiceClient
    {
        Task CreateUserAsync(User user, CancellationToken token = default);
    }
}
