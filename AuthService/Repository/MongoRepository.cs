using AuthService.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthService.Repository
{
    public class MongoRepository : IRepository
    {
        protected readonly IMongoClient _client;
        protected readonly IMongoDatabase _database;

        public MongoRepository(IOptions<MongoDbRepositoryConfiguration> optionsAccessor)
        {
            MongoDbRepositoryConfiguration configurationOptions = optionsAccessor.Value;

            _client = new MongoClient(configurationOptions.ConnectionString);
            _database = _client.GetDatabase(configurationOptions.DatabaseName);
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return _database.GetCollection<T>(typeof(T).Name).AsQueryable();
        }

        public async Task<IEnumerable<T>> AllAsync<T>() where T : class, new()
        {
            IAsyncCursor<T> asyncCursor = await _database.GetCollection<T>(typeof(T).Name).FindAsync(Builders<T>.Filter.Empty);
            return await asyncCursor.ToListAsync();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression);
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            IAsyncCursor<T> asyncCursor = await _database.GetCollection<T>(typeof(T).Name).FindAsync((FilterDefinition<T>)expression);
            return await asyncCursor.ToListAsync();
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).DeleteMany(predicate);
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name).DeleteManyAsync(predicate);
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public async Task<T> SingleAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            IAsyncCursor<T> asyncCursor = await _database.GetCollection<T>(typeof(T).Name).FindAsync((FilterDefinition<T>)expression);
            return await asyncCursor.SingleOrDefaultAsync();
        }

        public bool CollectionExists<T>() where T : class, new()
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(typeof(T).Name);
            long totalCount = collection.CountDocuments(Builders<T>.Filter.Empty);
            return totalCount > 0;
        }

        public void Add<T>(T item) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertOne(item);
        }

        public async Task AddAsync<T>(T item) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name).InsertOneAsync(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertMany(items);
        }

        public async Task<T> UpdateAsync<T>(Expression<Func<T, bool>> filter, T item) where T : class, new()
        {
            await _database.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, item, new ReplaceOptions
            {
                IsUpsert = false
            });

            return item;
        }
    }
}
