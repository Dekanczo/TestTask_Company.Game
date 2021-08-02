using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.DAL.Models;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Game.API.Controllers
{
    [ApiController]
    public class UsersController : GeneralController
    {
        public UsersController(ApplicationContext context) : base(context) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _context.Users
                .Include(t=>t.Gamers)
                .ToArrayAsync();

            return new JsonResult(users)
            {
                StatusCode = Ok().StatusCode
            };
        }
    }
}
