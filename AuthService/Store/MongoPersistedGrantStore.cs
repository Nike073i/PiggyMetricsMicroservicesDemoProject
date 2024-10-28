using AuthService.Repository;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthService.Store
{
    /// <summary>
    /// Репозиторий кодов авторизации, токенов обновления
    /// </summary>
    public class MongoPersistedGrantStore : IPersistedGrantStore
    {
        protected IRepository _db;

        public MongoPersistedGrantStore(IRepository db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return await _db.FindAsync<PersistedGrant>(i => i.SubjectId == subjectId);
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            if (string.IsNullOrEmpty(filter.Type) &&
                string.IsNullOrEmpty(filter.ClientId) &&
                string.IsNullOrEmpty(filter.SessionId) &&
                string.IsNullOrEmpty(filter.SubjectId))
            {
                return Enumerable.Empty<PersistedGrant>();
            }

            return await _db.FindAsync(BuildExpressionFilter(filter));
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await _db.SingleAsync<PersistedGrant>(i => i.Key == key);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            await _db.DeleteAsync<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            await _db.DeleteAsync<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId && i.Type == type);
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            if (string.IsNullOrEmpty(filter.Type) &&
                string.IsNullOrEmpty(filter.ClientId) &&
                string.IsNullOrEmpty(filter.SessionId) &&
                string.IsNullOrEmpty(filter.SubjectId))
            {
                return;
            }

            await _db.DeleteAsync(BuildExpressionFilter(filter));
        }

        public async Task RemoveAsync(string key)
        {
            await _db.DeleteAsync<PersistedGrant>(i => i.Key == key);
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            await _db.AddAsync<PersistedGrant>(grant);
        }

        private Expression<Func<PersistedGrant, bool>> BuildExpressionFilter(PersistedGrantFilter filter)
        {
            Expression expression = Expression.Empty();

            ParameterExpression parameterExpression = Expression.Parameter(typeof(PersistedGrant), "i");

            if (!string.IsNullOrEmpty(filter.Type))
            {
                Expression<Func<PersistedGrant, bool>> typeEqualExpression =
                    Expression.Lambda<Func<PersistedGrant, bool>>(Expression.Equal(Expression.PropertyOrField(parameterExpression, nameof(PersistedGrant.Type)), Expression.Constant(filter.Type)));

                expression = Expression.AndAlso(expression, typeEqualExpression);
            }

            if (!string.IsNullOrEmpty(filter.ClientId))
            {
                Expression<Func<PersistedGrant, bool>> typeEqualExpression =
                    Expression.Lambda<Func<PersistedGrant, bool>>(Expression.Equal(Expression.PropertyOrField(parameterExpression, nameof(PersistedGrant.ClientId)), Expression.Constant(filter.ClientId)));

                expression = Expression.AndAlso(expression, typeEqualExpression);
            }

            if (!string.IsNullOrEmpty(filter.SessionId))
            {
                Expression<Func<PersistedGrant, bool>> typeEqualExpression =
                    Expression.Lambda<Func<PersistedGrant, bool>>(Expression.Equal(Expression.PropertyOrField(parameterExpression, nameof(PersistedGrant.SessionId)), Expression.Constant(filter.SessionId)));

                expression = Expression.AndAlso(expression, typeEqualExpression);
            }

            if (!string.IsNullOrEmpty(filter.SubjectId))
            {
                Expression<Func<PersistedGrant, bool>> typeEqualExpression =
                    Expression.Lambda<Func<PersistedGrant, bool>>(Expression.Equal(Expression.PropertyOrField(parameterExpression, nameof(PersistedGrant.SubjectId)), Expression.Constant(filter.SubjectId)));

                expression = Expression.AndAlso(expression, typeEqualExpression);
            }

            return Expression.Lambda<Func<PersistedGrant, bool>>(expression, parameterExpression);
        }
    }
}
