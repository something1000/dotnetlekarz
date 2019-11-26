using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public class UserService : IUserService
    {
        //private static readonly UserService singletonUserService = new UserService();
        private DocDbContext _dbContext { get; }
        private List<UserModel> users { get; }
        private int lastId = 0;

        public UserService(DocDbContext dbContext)
        {
            _dbContext = dbContext;
            users = new List<UserModel>();
        }

        //public static UserService GetInstance()
        //{
        //    return singletonUserService;
        //}

        public void AddUser(UserModel user)
        {
            //user.id = Interlocked.Increment(ref lastId);
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

        public void ModifyUser(UserModel user)
        {
            _dbContext.Update(user);
            //var foundUser = users.FindIndex(x => x.id == user.id);
            //if(foundUser >= 0)
            //{
            //    users[foundUser] = user;
            //}
            _dbContext.SaveChanges();
        }

        public UserModel GetUser(int id)
        {
            var foundUser = _dbContext.Users.Find(id);
            //if(foundUser == null)
            //{
            //    return null;
            //}
            return foundUser;
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> listCopy = _dbContext.Users.ToList();

            return listCopy;
        }

        public UserModel GetUserByLogin(string login)
        {
            var foundUser = _dbContext.Users.Where(v => v.login.Equals(login)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            return foundUser;
        }
    }
}
