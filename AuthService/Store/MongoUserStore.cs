using AuthService.Domain;
using AuthService.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Store
{
    public class MongoUserStore : IUserStore<User>
    {
        private readonly IRepository _db;
        private readonly ILookupNormalizer _normalizer;

        public MongoUserStore(IRepository db, ILookupNormalizer normalizer)
        {
            _db = db;
            _normalizer = normalizer;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            User dbUser = await _db.SingleAsync<User>(x => x.Id == user.Id);
            if (dbUser == null)
                return null;

            return dbUser.Id.ToString("N");
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            User dbUser = await _db.SingleAsync<User>(x => x.Id == user.Id);
            return dbUser?.UserName;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            await _db.UpdateAsync(x => x.Id == user.Id, user);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_normalizer.NormalizeName(user.UserName));
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _db.AddAsync(user);
            return new IdentityResult();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await _db.UpdateAsync(x => x.Id == user.Id, user);
            return new IdentityResult();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await _db.DeleteAsync<User>(x => x.Id == user.Id);
            return new IdentityResult();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(userId);
            User dbUser = await _db.SingleAsync<User>(x => x.Id == id);
            return dbUser;
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            User dbUser = await _db.SingleAsync<User>(x => x.UserName == normalizedUserName);
            return dbUser;
        }

        public void Dispose()
        {
        }
    }
}
