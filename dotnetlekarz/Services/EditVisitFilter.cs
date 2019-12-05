using dotnetlekarz.Models;
using dotnetlekarz.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnetlekarz.Controllers
{
    public class EditVisitFilter : IActionFilter
    {
        private readonly ILogger<VisitController> _logger;
        private readonly IVisitService _visitService;
        public EditVisitFilter(ILogger<VisitController> logger, IVisitService service)
        {
            _logger = logger;
            _visitService = service;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string userLogin = context.HttpContext.User.Identity.Name;
            int visitID = (int)context.ActionArguments["id"];
            Visit visit = _visitService.GetVisit(visitID);
            if (userLogin != visit.Doctor.Login && userLogin != visit.Visitor.Login)
            {
                _logger.LogWarning("User: {login} tried to edit another user visit", userLogin);
                context.Result = new RedirectToActionResult("Index", "Visit", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
