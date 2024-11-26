using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SmartAssistance.Models;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task<IActionResult> ValidateAttendanceToday()
        {
            DateTime today = DateTime.Now.Date;

            string validation = "MARCAR";

            var assistRecord = await context.Set<Assist>()
                .Where(a => a.EmployeesId == GetPersonId() &&
                (a.CheckIn.HasValue || a.CheckOut.HasValue) &&
                (a.CheckIn.Value.Date == today || a.CheckOut.Value.Date == today))
                .FirstOrDefaultAsync();

            if (assistRecord != null)
            {
                if (assistRecord.CheckIn.HasValue && assistRecord.CheckOut.HasValue &&
                    assistRecord.CheckIn.Value.Date == today && assistRecord.CheckOut.Value.Date == today)
                    validation = "BLOQUEADO";

                else if (assistRecord.CheckIn.HasValue && assistRecord.CheckIn.Value.Date == today)
                    validation = "SALIDA";
            }

            return Content(JsonConvert.SerializeObject
                (validation), "application/json");
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