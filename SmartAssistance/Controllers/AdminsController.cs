﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using SmartAssistance.Models;

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

        [HttpGet]
        public async Task<IActionResult> MarkAttendance
            (string markType)
        {
            var result = false;

            if (markType == "ENTRADA")
            {
                var today = DateTime.Now;

                var limitHour = new DateTime
                    (today.Year, today.Month, today.Day, 9, 0, 0);

                var minuteLate = 0;

                if (today > limitHour)
                {
                    TimeSpan difference = today - limitHour;

                    minuteLate = (int)difference.TotalMinutes;
                }

                var assist = await context.Set<Assist>()
                    .Where(a => a.EmployeesId == GetPersonId() &&
                    a.CheckIn.HasValue &&
                    a.CheckIn.Value.Date == today.Date)
                    .FirstOrDefaultAsync();

                if (assist == null)
                {
                    await context.Set<Assist>().AddAsync
                    (new(GetPersonId(), DateTime.Now, null, minuteLate, "FELIZ"));

                    await context.SaveChangesAsync();

                    result = true;
                }
            }
            else if (markType == "SALIDA")
            {
                var assist = await context.Set<Assist>()
                    .Where(a => a.EmployeesId == GetPersonId() &&
                    a.CheckOut == null).FirstOrDefaultAsync();

                if (assist != null)
                {
                    result = await context.Set<Assist>().Where(a => a.Id == assist.Id)
                    .ExecuteUpdateAsync(a => a
                    .SetProperty(u => u.CheckOut, DateTime.Now)) > 0;
                }
            }

            return Content(JsonConvert.SerializeObject
                (result), "application/json");
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