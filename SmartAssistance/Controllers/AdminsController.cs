using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartAssistance.Models;
using System.Security.Claims;

namespace SmartAssistance.Controllers
{
    public class AdminsController
        (SmartAssistanceContext context) :
        Controller
    {
        private ClaimsPrincipal? _claimsPrincipal;

        #region Views

        [HttpGet]
        public async Task<IActionResult> InterfaceAdmin()
        {
            ViewBag.Admin = await
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
                     Area = ar.Name,
                     Position = po.Name,
                     Specialty = es.Name,
                 }
                ).FirstOrDefaultAsync();

            return View();
        }

        #endregion

        #region Json

        [HttpGet]
        public async Task<IActionResult> ValidateAttendanceToday()
        {
            DateTime today = DateTime.Now.Date;

            string validation = "MARCAR";

            var assist = await context.Set<Assist>()
                .Where(a => a.EmployeesId == GetPersonId() &&
                (a.CheckIn.HasValue || a.CheckOut.HasValue) &&
                (a.CheckIn.Value.Date == today || a.CheckOut.Value.Date == today))
                .FirstOrDefaultAsync();

            if (assist != null)
            {
                if (assist.CheckIn.HasValue && assist.CheckOut.HasValue &&
                    assist.CheckIn.Value.Date == today && assist.CheckOut.Value.Date == today)
                {
                    validation = "BLOQUEADO";
                }
                else if (assist.CheckIn.HasValue && assist.CheckIn.Value.Date == today)
                {
                    validation = "SALIDA";
                }
            }

            return Content(JsonConvert.SerializeObject
                (validation), "application/json");
        }

        #endregion

        #region Cookie

        private string GetPersonId()
        {
            _claimsPrincipal = HttpContext.User;

            return _claimsPrincipal
                .FindFirst(ClaimTypes.Name)?
                .Value.ToString() ?? string.Empty;
        }

        #endregion
    }
}