﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Authorization;
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

        private string GetUserName()
        {
            return HttpContext.User.Identity.Name;
        }

        // GET: Visit
        public ActionResult Index()
        {
            if (HttpContext.User.IsInRole("Doctor"))
                return View(viewName: "DocVisits", model: _visitService.GetVisitsByDoctor(GetUserName()));
            return View(_visitService.GetVisitsByVisitor(GetUserName()));
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
            if (TempData["docLogin"] == null || TempData["date"] == null)
                return RedirectToAction(nameof(Create));

            User doctor = _userService.GetUserByLogin(TempData["docLogin"].ToString());
            DateTime date = Convert.ToDateTime(TempData["date"].ToString());
            List < Visit > visits = _visitService.GetVisitsDocDate(doctor, date);

            List<string> hours = new List<string>{ "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
                                                   "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30" };

            foreach (Visit v in visits)
            {
                string hour = v.DateTime.ToString("HH:mm");//.Split(" ")[1].Remove(5);
                hours.RemoveAll(h => h.Equals(hour));

            }

            TempData.Peek("date");
            TempData.Peek("docLogin");
            TempData.Peek("edit");
            if (HttpContext.User.IsInRole("Doctor"))
            {
                TempData.Peek("visitorLogin");
            }

            return View(viewName: "VisitHour", model: hours);
        }

        // POST: Visit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                User visitor, doctor;
                if (HttpContext.User.IsInRole("Doctor"))
                {
                    visitor = _userService.GetUserByLogin(collection["visitorLogin"].ToString());
                    doctor = _userService.GetUserByLogin(GetUserName());
                }
                else
                {
                    doctor = _userService.GetUserByLogin(collection["docLogin"].ToString());
                    visitor = _userService.GetUserByLogin(GetUserName());
                }
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

                if (HttpContext.User.IsInRole("Doctor"))
                {
                    TempData["visitorLogin"] = collection["visitor"].ToString();
                }

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

        [Authorize(Roles="Doctor")]
        public IActionResult GenerateTodaysVisitsToPDF()
        {
            User user = _userService.GetUserByLogin(this.HttpContext.User.Identity.Name);
            if (user == null) return new StatusCodeResult(500);

            var Renderer = new IronPdf.HtmlToPdf();

            String HtmlContent = @"
                                <h1>Dzisiejsze wizyty {0}</h1>
                                <small>Godzina wygenerowania: {1} </small>
                                <div style=""margin:10px;"">
                                    <table width=""100%"" style=""border: 1px solid #000;"">
                                      <tr>
                                        <th>Pacjent</th>
                                        <th>Dzień</th>
                                        <th>Godzina</th>
                                      </tr>
                                      {2}
                                    </table>
                                </div>
            ";

            var doctorVisits = _visitService.GetVisitsDocDate(user, DateTime.Now);

            StringBuilder rowBuilder = new StringBuilder(500);
            String row = @"<tr>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td>{2}</td>
                           </tr>";

            foreach (var v in doctorVisits)
            {
                rowBuilder.Append(String.Format(row,
                                                v.Visitor.FullName(),
                                                v.DateTime.ToString("dd-MM-yyyy"),
                                                v.DateTime.ToString("HH:mm"))

                );
            }
            var PDF = Renderer.RenderHtmlAsPdf(String.Format(HtmlContent,
                                                             user.FullName(),
                                                             DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                                                             rowBuilder.ToString()));
            //return a  pdf document from a view
            var contentLength = PDF.BinaryData.Length;

            return new FileStreamResult(PDF.Stream, "application/pdf");
        }
    }
}