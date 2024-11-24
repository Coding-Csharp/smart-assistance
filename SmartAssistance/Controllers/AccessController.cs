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

        [HttpPost]
        public async Task<IActionResult> Login
            (User credential)
        {
            var result = await
                (from ec in context.Set<EmployeeCredential>()
                 join em in context.Set<Employee>()
                 on ec.EmployeesId equals em.Id
                 where ec.EmployeesId == credential.Username &&
                 ec.Code == credential.Password &&
                 em.State == "ACTIVO"
                 select em
                ).FirstOrDefaultAsync() != null;

            if (result is true)
            {
                List<Claim> claims =
                [
                    new(ClaimTypes.Role, credential.Role ?? string.Empty),
                    new(ClaimTypes.Name, credential.Username ?? string.Empty)
                ];

                ClaimsIdentity claimsIdentity = new(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
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