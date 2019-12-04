using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Authorization;
using IronPdf;
using System.Text;

namespace dotnetlekarz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IVisitService _visitService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IVisitService service, IUserService userService)
        {
            _logger = logger;
            _visitService = service;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Doctor")]
        [Route("Policy")]
        public IActionResult Privacy()
        {
            Console.WriteLine(HttpContext.User.Claims);
            return View(_visitService.GetAllVisits());
        }

        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
