using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnetlekarz.Controllers
{
    public class VisitController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IVisitService _visitService;
        private readonly IUserService _userService;

        public VisitController(ILogger<HomeController> logger, IVisitService service, IUserService userService)
        {
            _logger = logger;
            _visitService = service;
            _userService = userService;
        }

        // GET: Visit
        public ActionResult Index()
        {
            return View(_visitService.GetAllVisits());
        }

        // GET: Visit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Visit/Create
        public ActionResult Create()
        {
            List<User> doctors = _userService.GetAllUsers().FindAll(x => x.UserRole.Equals(Models.User.Role.Doctor));
            return View(doctors);
        }

        // POST: Visit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Visit/Edit/5
        public ActionResult Edit(int id)
        {
            Visit model = _visitService.GetVisit(id);
            return View(model);
        }

        // POST: Visit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // DELETE: Visit/Delete/5
        public ActionResult Delete(int id)
        {
            _visitService.RemoveVisit(id);
            return RedirectToAction("Index", "Visit");
        }

        // POST: Visit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}