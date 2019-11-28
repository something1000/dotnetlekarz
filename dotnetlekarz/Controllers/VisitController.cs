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

        // GET: Visit/Hour
        public ActionResult Hour()
        {
            User doctor = _userService.GetUserByLogin(TempData["docLogin"].ToString());
            DateTime date = Convert.ToDateTime(TempData["date"].ToString());
            List < Visit > visits = _visitService.GetVisitsDocDate(doctor, date);

            List<string> hours = new List<string>{ "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30" };
            
            if (visits.Count > 0)
            {
                foreach (Visit v in visits)
                {
                    string hour = v.DateTime.ToString().Split(" ")[1].Remove(5);
                    hours.RemoveAll(h => h.Equals(hour));
                }
            }

            TempData.Peek("date");
            TempData.Peek("docLogin");
            TempData.Peek("edit");
            return View(viewName: "VisitHour", model: hours);
        }

        // POST: Visit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                User doctor = _userService.GetUserByLogin(collection["docLogin"].ToString());
                User visitor = _userService.GetUserByLogin("kjablonski"); ///////////////////////////// zmienić jak będzie logowanie
                string onlyDate = collection["date"].ToString().Split(" ")[0];
                DateTime date = Convert.ToDateTime(onlyDate + (" ") + collection["hour"].ToString() + ":00");

                _visitService.AddVisit(new Visit(doctor, visitor, date));

                if (!collection["edit"].ToString().Equals("-1"))    // znaczy że to była edycja 
                {
                    return RedirectToAction(nameof(Delete), new { id = collection["edit"] });
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Visit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChooseDate(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                TempData["docLogin"] = collection["doctor"].ToString();
                TempData["date"] = collection["date"].ToString();
                TempData["edit"] = collection["edit"].ToString();

                return RedirectToAction(nameof(Hour));
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
            return RedirectToAction(nameof(Index));
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