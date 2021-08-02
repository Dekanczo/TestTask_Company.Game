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
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Controllers
{
    [ApiController]
    public class CharactersController : GeneralController
    {
        public CharactersController(ApplicationContext context) : base(context) { }


        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> Character(Guid id)
        {
            var user = await _context.Characters
                .Include(c => c.CharacterWeaponRelations)
                .ThenInclude(c => c.Weapon)
                .FirstAsync(p => p.Id == id);

            return new JsonResult(user)
            {
                StatusCode = Ok().StatusCode
            };
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> AllCharacters()
        {
            var characters = await _context.Characters
                    .Include(c => c.CharacterWeaponRelations)
                    .ThenInclude(t => t.Weapon)
                    .ToArrayAsync();

            return new JsonResult(characters)
            {
                StatusCode = Ok().StatusCode
            };
        }
    }
}
