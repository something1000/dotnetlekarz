using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace DocTests
{
    public class UserServiceTest
    {

        [Fact]
        public void GetUserFromUserListByLoginAndCheckIfAllFieldsAreValidAndPasswordIsHashed()
        {
            DocDbContext db = TestDbContext.GetDocDbContext();
            UserService us = new UserService(db);

            us.AddUser(new User
            {
                Login = "login1",
                Password = "pass1",
                Name = "name1",
                Surname = "surname1"
            });

            User user = us.GetUserByLogin("login1");
            Console.WriteLine(user.Password);
            Assert.NotNull(user);
            Assert.Equal("login1", user.Login);

            Assert.Equal(User.HashPassword("pass1"), user.Password);
            Assert.Equal("name1", user.Name);
            Assert.Equal("surname1", user.Surname);
        }


        [Fact]
        async public void AddUserToDatabaseThenFindHimByFunctionThenRemoveHimAndTryToFindAgain()
        {
            DocDbContext db = TestDbContext.GetDocDbContext();
            UserService us = new UserService(db);

            us.AddUser(new User
            {
                Login = "login2",
                Password = "pass2",
                Name = "name2",
                Surname = "surname2"
            });

            us.AddUser(new User
            {
                Login = "login3",
                Password = "pass3",
                Name = "name3",
                Surname = "surname3"
            });

            us.AddUser(new User
            {
                Login = "login4",
                Password = "pass4",
                Name = "name4",
                Surname = "surname4"
            });

            int userCount = await db.Users.CountAsync();
            Assert.Equal(3, userCount);

            us.AddUser(new User
            {
                Login = "login6",
                Password = "pass6",
                Name = "name6",
                Surname = "surname6"
            });

            User u = us.GetUserByLogin("login6");
            Assert.NotNull(u);

            Assert.True(us.RemoveUser(u.UserId));

            User u1 = us.GetUserByLogin("login6");
            Assert.Null(u1);
        }
    }
}
