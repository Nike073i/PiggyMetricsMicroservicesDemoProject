using AccountService.Domain;
using System.Threading.Tasks;

namespace AccountService.Repository
{
    /// <summary>
    /// Account repository
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Find account by name
        /// </summary>
        /// <param name="name">Account name</param>
        /// <returns></returns>
        Task<Account> FindByName(string name);

        /// <summary>
        /// Save account changes
        /// </summary>
        /// <param name="account">Account</param>
        /// <returns></returns>
        Task Save(Account account);
    }
}
