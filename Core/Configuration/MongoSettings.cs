namespace Core.Configuration
{
    /// <summary>
    /// MongoDB connection settings
    /// </summary>
    public class MongoSettings
    {
        /// <summary>
        /// Collection name
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Connection string with credentials
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
