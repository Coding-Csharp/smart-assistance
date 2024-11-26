using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SmartAssistance.Models;

namespace SmartAssistance.Controllers
{
    public class EmployeesController
        (SmartAssistanceContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        [HttpGet]
        public async Task<IActionResult> InterfaceEmployee()
        {
            ViewBag.Profile = await
                (from em in context.Set<Employee>()
                 join es in context.Set<Specialty>()
                 on em.SpecialtiesId equals es.Id
                 join an in context.Set<Assign>()
                 on em.Id equals an.EmployeesId
                 join po in context.Set<Position>()
                 on an.PositionsId equals po.Id
                 join ar in context.Set<Area>()
                 on po.AreasId equals ar.Id
                 where em.Id == GetPersonId()
                 select new
                 {
                     em.Id,
                     em.DateEntry,
                     em.TypeDocument,
                     em.Firstname,
                     em.Lastname,
                     em.Birthdate,
                     em.Nationality,
                     em.Genre,
                     em.Phone,
                     em.Email,
                     em.Address,
                     em.ZoneAccess,
                     Area = ar.Name,
                     Position = po.Name,
                     Specialty = es.Name,
                 }
                ).FirstOrDefaultAsync();

            return View();
        }

        [HttpGet]
        public IActionResult AttendanceList() => View();

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }
    }
}