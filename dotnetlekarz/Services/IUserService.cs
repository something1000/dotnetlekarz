using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    interface IUserService
    {
        void AddUser(UserModel user);

        bool RemoveUser(int id);

        void ModifyUser(UserModel user);

        UserModel GetUser(int id);

        List<UserModel> GetAllUsers();
    }
}
