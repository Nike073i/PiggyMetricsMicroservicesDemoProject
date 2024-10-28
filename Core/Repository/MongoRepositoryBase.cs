using MongoDB.Driver;

namespace Core.Repository
{
    /// <summary>
    /// Base mongodb repository class
    /// </summary>
    /// <typeparam name="T">Collection model</typeparam>
    public abstract class MongoRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Collection
        /// </summary>
        protected readonly IMongoCollection<T> Collection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <param name="connectionString">Connection string</param>
        protected MongoRepositoryBase(string collectionName, string connectionString)
        {
            var client = new MongoClient(connectionString);
            var mongoUrl = MongoUrl.Create(connectionString);
            IMongoDatabase database = client.GetDatabase(mongoUrl.DatabaseName);
            Collection = database.GetCollection<T>(collectionName);
        }
    }
}
