using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model
{
    public class User
    {
        [BsonId]
        public ObjectId ID { get; set; }
        // external Id, easier to reference: 1,2,3 or A, B, C etc.
        public string Id { get; set; }         
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Role> Roles {get;set;}
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public class UserDto
    {
        [BsonId]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserInfo
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }    
}