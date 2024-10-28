using AccountService.Domain;
using Core.Configuration;
using Core.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AccountService.Repository
{
    ///<inheritdoc/>
    public class AccountRepository : MongoRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(IOptions<MongoSettings> options) 
            : base(options.Value.CollectionName, options.Value.ConnectionString)
        {
        }

        ///<inheritdoc/>
        public async Task<Account> FindByName(string name)
        {
            IAsyncCursor<Account> queryResult = await Collection.FindAsync(Builders<Account>.Filter.Eq(x => x.Name, name));
            return await queryResult.FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task Save(Account account)
        {
            await Collection.ReplaceOneAsync(Builders<Account>.Filter.Eq(x => x.Name, account.Name), account, new ReplaceOptions
            {
                IsUpsert = true
            });
        }
    }
}
