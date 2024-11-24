using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using SmartAssistance.Models;

namespace SmartAssistance.Controllers
{
    [AllowAnonymous]
    public class AccessController
        (SmartAssistanceContext context,
        IWebHostEnvironment webHostEnvironment) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated is true)
            {
                if (User.IsInRole("ADMINISTRADOR"))
                    return RedirectToAction("InterfaceAdmin", "Admins");

                else if (User.IsInRole("TRABAJADOR"))
                    return RedirectToAction("InterfaceEmployee", "Employees");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }

        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            _claimsPrincipal = HttpContext.User;

            var personId = _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;

            var employee = await context.Set<Employee>()
                .Where(a => a.Id == personId)
                .FirstOrDefaultAsync();

            return Content(JsonConvert.SerializeObject
                (employee), "application/json");
        }
    }
}