using dotnetlekarz.Controllers;
using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DocTests
{
    public class AccountControllerTest
    {
        private ITestOutputHelper output;
        public AccountControllerTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async void CreateUsertThanTryToLoginWithProvidedCredentitials()
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


            AccountController ac = new AccountController(new Mock<ILogger<AccountController>>().Object, us, null);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            //urlHelperFactory
            //    .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            //    .Returns(Task.FromResult((object)null));

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);
            //serviceProviderMock
            //    .Setup(_ => _.GetService(typeof(ITempDataDictionaryFactory)))
            //    .Returns(new Mock<ITempDataDictionaryFactory>().Object);

            ac.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProviderMock.Object
                }
            };

            var res = await ac.Login("login1", "pass1");
            output.WriteLine(res.GetType().ToString());
            Assert.NotNull(res);
            Assert.Equal(typeof(RedirectToActionResult), res.GetType());

            Assert.True(true);
        }
    }
}
