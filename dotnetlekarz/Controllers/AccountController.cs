using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnetlekarz.Controllers
{
    public class AccountController : Controller
    {
        private bool loggedIn = false;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Submit()
        {
            //TODO authenticate
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}