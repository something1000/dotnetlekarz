using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace dotnetlekarz.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private IUserService _userService { get; set; }
        private IVisitService _visitService { get; set; }

        public AccountController(ILogger<AccountController> logger, IUserService userService, IVisitService visitService)
        {
            _logger = logger;
            _userService = userService;
            _visitService = visitService;
        }

        private string GetUserName()
        {
            return HttpContext.User.Identity.Name;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserRole = Models.User.Role.Visitor;
                try
                {
                    _userService.AddUser(user);
                    _logger.LogInformation("User: {login} has registered", user.Login);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    TempData["LoginExists"] = "User with login: " + user.Login + " already exists";
                    return View();
                }

                var claimsPrincipal = CreateClaimsPrincipal(user);
                var authProperties = new AuthenticationProperties
                {

                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Some fields are invalid");
            return View();
        }

        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(string Login, string Password)
        {
            if (ModelState.IsValid)
            {
                User validatedUser = ValidateUserLogin(Login, Password);
                if(validatedUser != null)
                {
                    var claimsPrincipal = CreateClaimsPrincipal(validatedUser);
                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        claimsPrincipal,
                        authProperties);
                    _logger.LogInformation("User: {login} logged in", Login);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        private User ValidateUserLogin(string Login, string Password)
        {
            var registeredUser = _userService.GetUserByLogin(Login);
            var hashedPassword = Models.User.HashPassword(Password);
            if(registeredUser != null && registeredUser.Password == hashedPassword)
            {
                return registeredUser;
            }
            return null;
        }

        private ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.getUserRole()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _logger.LogInformation("User: {login} logged out", GetUserName());
            return RedirectToAction("Login", "Account");
        }

    }
}