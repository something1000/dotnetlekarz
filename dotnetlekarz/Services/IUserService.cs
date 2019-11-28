using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public interface IUserService
    {
        void AddUser(User user);

        bool RemoveUser(int id);

        void ModifyUser(User user);

        User GetUser(int id);
        User GetUserByLogin(String login);
        User GetUserByID(int id);
        List<User> GetAllUsers();
    }
}
