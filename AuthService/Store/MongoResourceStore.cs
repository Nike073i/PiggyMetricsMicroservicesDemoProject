using AuthService.Repository;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Store
{
    /// <summary>
    /// Репозиторий ресурсов идентификации
    /// </summary>
    public class MongoResourceStore : IResourceStore
    {
        protected IRepository _db;

        public MongoResourceStore(IRepository db)
        {
            _db = db;
        }

        private async Task<IEnumerable<ApiResource>> GetAllApiResources()
        {
            return await _db.AllAsync<ApiResource>();
        }

        private async Task<IEnumerable<IdentityResource>> GetAllIdentityResources()
        {
            return await _db.AllAsync<IdentityResource>();
        }

        private async Task<IEnumerable<ApiScope>> GetAllApiScope()
        {
            return await _db.AllAsync<ApiScope>();
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return await _db.SingleAsync<ApiResource>(a => a.Name == name);
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            IEnumerable<IdentityResource> identityResources = await GetAllIdentityResources();
            IEnumerable<ApiResource> apiResources = await GetAllApiResources();
            IEnumerable<ApiScope> apiScope = await GetAllApiScope();
            return new Resources(identityResources, apiResources, apiScope);
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return await _db.FindAsync<IdentityResource>(e => scopeNames.Contains(e.Name));
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return await _db.FindAsync<ApiScope>(a => scopeNames.Contains(a.Name));
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return await _db.FindAsync<ApiResource>(a => a.Scopes.Any(s => scopeNames.Contains(s)));
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return await _db.FindAsync<ApiResource>(a => apiResourceNames.Contains(a.Name));
        }
    }
}
