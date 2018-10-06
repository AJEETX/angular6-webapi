using System;
using System.Collections.Generic;
using System.Linq;
using DB;
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
        private DB.DataContext _context;

        public UserService(DB.DataContext context)
        {
            _context = context;
        }
        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(u => u.Username == user.Username))
                throw new AppException("Username '" + user.Username + "' is already taken");
            try
            {    
                byte[] passwordHash, passwordSalt;
                PasswordHasher.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.Roles=new List<Role>{new Role{Name= "Admin" }};
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (AppException)
            {
                //shout/catch/throw/log
            }
            return user;
        }
        
        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            try
            {    
            
                var user = _context.Users.SingleOrDefault(x => x.Username == username);

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
            return _context.Users.ToList();
        }

        public bool UpdateUser(UserInfo userInfo)
        {
            var user=GetUserById(userInfo.Id);
            user.FirstName=userInfo.FirstName;
            user.LastName=userInfo.LastName;
            var update = _context.Users.Update(user);
            _context.SaveChanges();
            return update is null ? false : true;
        }
    }
}