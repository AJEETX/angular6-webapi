
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model
{
    public class Role{
        [BsonId]
        public ObjectId ID { get; set; }
        public string Name { get; set; }
    }
    
    public class Settings
    {
        public string ConnectionString;
        public string Database;
    }
}