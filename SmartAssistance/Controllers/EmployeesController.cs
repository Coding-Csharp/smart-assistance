using Microsoft.AspNetCore.Mvc;
using SmartAssistance.Models;
using System.Security.Claims;

namespace SmartAssistance.Controllers
{
    public class EmployeesController
        (SmartAssistanceContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        public IActionResult Index()
        {
            return View();
        }

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }
    }
}