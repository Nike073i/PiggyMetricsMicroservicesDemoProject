using AuthService.Repository;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Threading.Tasks;

namespace AuthService.Store
{
    /// <summary>
    /// Репозиторий клиентов авторизации
    /// </summary>
    public class MongoClientStore : IClientStore
    {
        protected IRepository _db;

        public MongoClientStore(IRepository db)
        {
            _db = db;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await _db.SingleAsync<Client>(c => c.ClientId == clientId);
        }
    }
}
