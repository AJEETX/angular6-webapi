using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Model;

namespace WebApi.Helpers
{
    public class DataContext
    {
        private readonly IMongoDatabase _database = null;

        public DataContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("User");
            }
        }
        public IMongoCollection<Product> Products
        {
            get
            {
                return _database.GetCollection<Product>("Product");
            }
        }
    }
}