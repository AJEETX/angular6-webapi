using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Helpers;
using WebApi.Model;

namespace WebApi.Services
{
    public interface IUserService
    {
        User Create(User user, string password);
        User Authenticate(string username, string password);
        List<User> GetUsers();
        User GetUserById(int id);
        bool UpdateUser(UserInfo user);
    }

    public class UserService : IUserService
    {
        private DataContext _context;

        public UserService(IOptions<Settings> settings)
        {
            _context = new DataContext(settings);
        }
        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Find(u => u.Username == user.Username).FirstOrDefault()!=null)
                throw new AppException("Username '" + user.Username + "' is already taken");
            try
            {    
                byte[] passwordHash, passwordSalt;
                PasswordHasher.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.Roles=new List<Role>{new Role{Name= "Admin" }};
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.InsertOne(user);
            }
            catch (AppException)
            {
                //shout/catch/throw/log
            }
            return user;
        }
        
        public User GetUserById(int id)
        {
            return _context.Users.Find(u=>u.Id==id)?.FirstOrDefault();
        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            try
            {    
            
                var user = _context.Users.Find(x => x.Username == username).FirstOrDefault();

                if (user == null)
                    return null;

                if (!PasswordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;
                return user;
            }
            catch (AppException)
            {
               return null; //shout/catch/throw/log
            }            
        }

        public List<User> GetUsers()
        {
            return _context.Users.Find(_=>true).ToList();
        }

        public bool UpdateUser(UserInfo userInfo)
        {
            var user=GetUserById(userInfo.Id);
            user.FirstName=userInfo.FirstName;
            user.LastName=userInfo.LastName;
            var filter = Builders<User>.Filter.Eq(s => s.Id, userInfo.Id);

            var update = Builders<User>.Update
                            .Set(s => s.FirstName, userInfo.FirstName)
                            .Set(s=>s.LastName,userInfo.LastName);

            var updateResult = _context.Users.UpdateOne(filter,update);
            return updateResult.IsAcknowledged && updateResult.MatchedCount>0;
        }
    }
}