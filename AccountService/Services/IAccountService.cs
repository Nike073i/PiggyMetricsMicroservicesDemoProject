using AccountService.Domain;
using System.Threading.Tasks;

namespace AccountService.Services
{
    /// <summary>
    /// Account management
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Finds account by given name
        /// </summary>
        /// <param name="accountName">accountName</param>
        /// <returns>found account</returns>
        Task<Account> FindByName(string accountName);

        /// <summary>
        /// Checks if account with the same name already exists
        /// Invokes Auth Service user creation 
        /// Creates new account with default parameters
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>created account</returns>
        Task<Account> Create(User user);

        /// <summary>
        /// Validates and applies incoming account updates
        /// Invokes Statistics Service update
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="update">update</param>
        Task SaveChanges(string name, Account update);
    }
}
