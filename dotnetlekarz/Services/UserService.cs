using dotnetlekarz.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public class UserService : IUserService
    {
        //private static readonly UserService singletonUserService = new UserService();
        private DocDbContext _dbContext { get; }
        private List<User> users { get; }

        public UserService(DocDbContext dbContext)
        {
            _dbContext = dbContext;
            users = new List<User>();
        }

        //public static UserService GetInstance()
        //{
        //    return singletonUserService;
        //}

        public void AddUser(User user)
        {
            //user.id = Interlocked.Increment(ref lastId);
            user.Password = User.HashPassword(user.Password);
            users.Add(user);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public bool RemoveUser(int id)
        {
            var toRemove = _dbContext.Users.Find(id);
            if(toRemove != null)
            {
                _dbContext.Remove(toRemove);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void ModifyUser(User user)
        {
            /// co z haslem [TODO]
            _dbContext.Update(user);
            //var foundUser = users.FindIndex(x => x.id == user.id);
            //if(foundUser >= 0)
            //{
            //    users[foundUser] = user;
            //}
            _dbContext.SaveChanges();
        }

        public User GetUser(int id)
        {
            var foundUser = _dbContext.Users.Find(id);
            //if(foundUser == null)
            //{
            //    return null;
            //}
            return foundUser;
        }

        public List<User> GetAllUsers()
        {
            List<User> listCopy = _dbContext.Users.ToList();

            return listCopy;
        }

        public User GetUserByLogin(string login)
        {
            var foundUser = _dbContext.Users.Where(v => v.Login.Equals(login)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            return foundUser;
        }

        public User GetUserByID(int id)
        {
            var foundUser = _dbContext.Users.Where(v => v.UserId.Equals(id)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            return foundUser;
        }
    }
}
