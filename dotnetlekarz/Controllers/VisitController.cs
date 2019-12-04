using System;
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
    [Route("Visit")]
    public class VisitController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IVisitService _visitService;
        private readonly IUserService _userService;

        private List<string> hours = new List<string>{ "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
                                                   "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30" };

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
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            if (HttpContext.User.IsInRole("Doctor"))
                return View(viewName: "DocVisits", model: _visitService.GetVisitsByDoctor(GetUserName()));
            return View(_visitService.GetVisitsByVisitor(GetUserName()));
        }


        // GET: Visit/Create
        [Route("Create")]
        public ActionResult Create()
        {
            List<User> doctors = _userService.GetAllUsers().FindAll(x => x.UserRole.Equals(Models.User.Role.Doctor));
            return View(doctors);
        }

        // GET: Visit/Hour
        [Route("Hour")]
        public ActionResult Hour()
        {
            if (TempData["docLogin"] == null || TempData["date"] == null)
                return RedirectToAction(nameof(Create));

            User doctor = _userService.GetUserByLogin(TempData["docLogin"].ToString());
            DateTime date = Convert.ToDateTime(TempData["date"].ToString());
            List < Visit > visits = _visitService.GetVisitsDocDate(doctor, date);

            List<string> availableHours = new List<string>(hours);

            foreach (Visit v in visits)
            {
                string hour = v.DateTime.ToString("HH:mm");//.Split(" ")[1].Remove(5);
                availableHours.RemoveAll(h => h.Equals(hour));

            }

            TempData.Peek("date");
            TempData.Peek("docLogin");
            TempData.Peek("edit");
            if (HttpContext.User.IsInRole("Doctor"))
            {
                TempData.Peek("visitorLogin");
            }

            return View(viewName: "VisitHour", model: availableHours);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                //WALIDACJA
                if (!hours.Contains(collection["hour"].ToString()))
                    TempData["validationHour"] = "Wybierz godzinę z listy";

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

                //Błędy walidacji
                if (TempData["validationHour"] != null)
                {
                    TempData["docLogin"] = collection["docLogin"].ToString();
                    TempData["date"] = collection["date"].ToString();
                    TempData["edit"] = collection["edit"].ToString();
                    if (HttpContext.User.IsInRole("Doctor"))
                        TempData["visitorLogin"] = collection["visitorLogin"].ToString();
                    return RedirectToAction(nameof(Hour));
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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                TempData["noAdd"] = "Wybrana godzina jest już zajęta, proszę wybrać inną";
                TempData["docLogin"] = collection["docLogin"].ToString();
                TempData["date"] = collection["date"].ToString();
                TempData["edit"] = collection["edit"].ToString();
                if(HttpContext.User.IsInRole("Doctor"))
                    TempData["visitorLogin"] = collection["visitorLogin"].ToString();
                return RedirectToAction(nameof(Hour));
            }
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ChooseDate")]
        public ActionResult ChooseDate(IFormCollection collection)
        {
            try
            {
                //WALIDACJA
                List<User> doctors = _userService.GetAllUsers().FindAll(x => x.UserRole.Equals(Models.User.Role.Doctor));
                bool docExists = false;
                foreach (User d in doctors)
                {
                    if (d.Login.Equals(collection["doctor"].ToString()))
                    {
                        docExists = true;
                        break;
                    }
                }
                if (!docExists)
                    TempData["validationDoctor"] = "Wybierz doktora z listy";
                try
                {
                    DateTime MyDateTime = DateTime.ParseExact(collection["date"].ToString(), "yyyy-MM-dd", null);
                    if (MyDateTime.CompareTo(DateTime.Now) <= 0)
                        TempData["validationDate"] = "Najbliższy możliwy termin zapisu to: " + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    else if (MyDateTime.DayOfWeek == DayOfWeek.Saturday || MyDateTime.DayOfWeek == DayOfWeek.Sunday)
                        TempData["validationDate"] = "Przychodnia czynna tylko od poniedziałku do piątku";
                }
                catch
                {
                    TempData["validationDate"] = "Poprawny format daty to: DD.MM.YYYY";
                }

                TempData["docLogin"] = collection["doctor"].ToString();
                TempData["date"] = collection["date"].ToString();
                TempData["edit"] = collection["edit"].ToString();

                if (HttpContext.User.IsInRole("Doctor"))
                {
                    TempData["visitorLogin"] = collection["visitor"].ToString();
                }
                
                //Błędy walidacji
                if (TempData["validationDoctor"] != null || TempData["validationDate"] != null)
                {
                    if (!collection["edit"].ToString().Equals("-1"))    // znaczy że to była edycja
                        return RedirectToAction(nameof(Edit), new { id = collection["edit"] });
                    return RedirectToAction(nameof(Create));
                }

                return RedirectToAction(nameof(Hour));
            }
            catch
            {
                return RedirectToAction(nameof(Create));
            }
        }


        // GET: Visit/Edit/5
        [Route("Edit")]
        public ActionResult Edit(int id)
        {
            Visit model = _visitService.GetVisit(id);
            return View(model);
        }

        // POST: Visit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
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
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            _visitService.RemoveVisit(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Visit/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
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
        [Route("GenerateTodaysVisitsToPDF")]
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