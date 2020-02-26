using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model
{
public class Product
{
    [BsonId]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Detail { get; set; }
    public bool Watch { get; set; }
    public string Time { get; set; }
    public string Location { get; set; }
    public int? Amountlost { get; set; }
    public string EventNo { get; set; }
}
    public class User
    {
        [BsonId]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Role> Roles {get;set;}
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
    public class Role{
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UserDto
    {
        [BsonId]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserInfo
    {
        [BsonId]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }    

    public class Settings
    {
        public string ConnectionString;
        public string Database;
    }
}