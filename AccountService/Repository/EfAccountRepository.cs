using System;
using System.Threading.Tasks;
using AccountService.Data.Contexts;
using AccountService.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace AccountService.Repository
{
    public class EfAccountRepository : IAccountRepository
    {
        private readonly AccountDbContext _dbContext;

        public EfAccountRepository(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> FindByName(string name)
        {
            return await _dbContext
                .Accounts
                .Include(a => a.Incomes)
                .Include(a => a.Saving)
                .Include(a => a.Expenses)
                .FirstOrDefaultAsync(a => a.Name.Equals(name));
        }

        public async Task Save(Account account)
        {
            var storedAccount = await FindByName(account.Name);
            if (storedAccount == null)
                _dbContext.Accounts.Add(account);
            else
                _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}
