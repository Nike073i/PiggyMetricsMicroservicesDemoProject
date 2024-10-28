using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthService.Repository
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class, new();
        Task<IEnumerable<T>> AllAsync<T>() where T : class, new();
        IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<T> SingleAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Add<T>(T item) where T : class, new();
        Task AddAsync<T>(T item) where T : class, new();
        void Add<T>(IEnumerable<T> items) where T : class, new();
        Task<T> UpdateAsync<T>(Expression<Func<T, bool>> filter, T item) where T : class, new();
        bool CollectionExists<T>() where T : class, new();
    }
}
