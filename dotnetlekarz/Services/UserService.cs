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

        private List<UserModel> users { get; }
        private int lastId = 0;

        public UserService()
        {
            AddUser(new UserModel("Jan", "Kowalski", "jkowalski", "123", UserModel.Role.Doctor));
            AddUser(new UserModel("Wojciech", "Rzezucha", "wrzezucha", "123", UserModel.Role.Doctor));
            AddUser(new UserModel("Bart", "Gawrych", "bgawrych", "123", UserModel.Role.Visitor));
            AddUser(new UserModel("Adam", "Grabowski", "agrabowski", "123", UserModel.Role.Visitor));
            AddUser(new UserModel("Kamil", "Jablonski", "kjablonski", "123", UserModel.Role.Visitor));
        }

        //public static UserService GetInstance()
        //{
        //    return singletonUserService;
        //}

        public void AddUser(UserModel user)
        {
            user.id = Interlocked.Increment(ref lastId);
            users.Add(user);
        }

        public bool RemoveUser(int id)
        {
            var toRemove = users.Find(x => x.id == id);
            if(toRemove != null)
            {
                users.Remove(toRemove);
                return true;
            }
            return false;
        }

        public void ModifyUser(UserModel user)
        {
            var foundUser = users.FindIndex(x => x.id == user.id);
            if(foundUser >= 0)
            {
                users[foundUser] = user;
            }
        }

        public UserModel GetUser(int id)
        {
            var foundUser = users.Find(v => v.id == id);
            if(foundUser == null)
            {
                return null;
            }
            return (UserModel)foundUser.Clone();
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> listCopy = new List<UserModel>();
            users.ForEach(v =>
            {
                listCopy.Add((UserModel)v.Clone());
            });
            return listCopy;
        }
    }
}
