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

namespace dotnetlekarz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IVisitService _visitService;

        public HomeController(ILogger<HomeController> logger, IVisitService service)
        {
            _logger = logger;
            _visitService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult Privacy()
        {
            Console.WriteLine(HttpContext.User.Claims);
            return View(_visitService.GetAllVisits());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
