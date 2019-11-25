﻿using dotnetlekarz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public interface IUserService
    {
        void AddUser(UserModel user);

        bool RemoveUser(int id);

        void ModifyUser(UserModel user);

        UserModel GetUser(int id);
        UserModel GetUserByLogin(String login);

        List<UserModel> GetAllUsers();
    }
}
