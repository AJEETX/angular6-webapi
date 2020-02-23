using System;
using System.Collections.Generic;

namespace WebApi.Model
{
public class Product
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Detail { get; set; }
    public bool Watch { get; set; }
    public string Time { get; set; }
    public string Location { get; set; }
}
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Role> Roles {get;set;}
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
    public class Role{
        public int Id { get; set; }
        public string Name { get; set; }
    }
        public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
        public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }    
}