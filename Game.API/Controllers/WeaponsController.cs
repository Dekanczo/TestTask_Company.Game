using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DAL.Models;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Game.API.Controllers
{
    [ApiController]
    public class WeaponsController : GeneralController
    {
        public WeaponsController(ApplicationContext context) : base(context) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weapon>>> Get()
        {
            var weapons = await _context.Weapons
                .ToArrayAsync();

            return new JsonResult(weapons)
            {
                StatusCode = Ok().StatusCode
            };
        }
    }
}
