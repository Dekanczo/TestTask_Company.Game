using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.DAL.Models;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Game.API.Models;
using System.Net.Http.Headers;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Game.API.Controllers
{
    [ApiController]
    public class GamersController : GeneralController
    {
        public GamersController(ApplicationContext context): base(context) { }

        [HttpGet]
        public async Task<ActionResult<Gamer>> Gamers()
        {
            var gamers = await _context.Gamers.ToArrayAsync();

            return new JsonResult(gamers)
            {
                StatusCode = Ok().StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gamer>> Gamer(string id)
        {
            var gamer = await _context.Gamers.FirstOrDefaultAsync(p => p.Id.ToString() == id);

            return new JsonResult(gamer)
            {
                StatusCode = Ok().StatusCode
            };
        }


        [HttpGet("{id}/Characters")]
        public async Task<ActionResult<IEnumerable<Gamer>>> Characters(string id)
        {
            var characterSet =
                from c in _context.Characters
                join gcr in _context.GamerCharacterRelations on c.Id equals gcr.CharacterId
                where gcr.GamerId.ToString() == id
                select c;

            var characters = await characterSet.ToArrayAsync();

            return new JsonResult(characters)
            {
                StatusCode = Ok().StatusCode
            };
        }

        [HttpGet("{id}/Weapons")]
        public async Task<ActionResult<IEnumerable<Gamer>>> Weapons(string id)
        {
            var cwrSet =
                from cwr in _context.CharacterWeaponRelations
                join gcr in _context.GamerCharacterRelations on cwr.CharacterId equals gcr.CharacterId
                where gcr.GamerId.ToString() == id
                select cwr.WeaponId.ToString();

            var weaponIds = await cwrSet.ToArrayAsync();

            var weapons = await _context.Weapons
                .Where(p => weaponIds.Contains(id))
                .ToArrayAsync();

            return new JsonResult(weapons)
            {
                StatusCode = Ok().StatusCode
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Gamer gamerData)
        {
            var gamer = new Gamer
            {
                Id = Guid.NewGuid(),
                Name = gamerData.Name,
                Level = gamerData.Level,
                UserId = Guid.Parse(User.Identity.Name)
            };

            await _context.Gamers.AddAsync(gamer);
            await _context.SaveChangesAsync();
            
            return new JsonResult(gamer)
            {
                StatusCode = Ok().StatusCode
            };
        }

        [HttpPost("BuyCharacter")]
        public async Task<ActionResult> BuyCharacter([FromBody] CharacterBuyingClaim claim)
        {
            var targetGamer = await _context.Gamers.AsTracking()
                .Include(o => o.GamerCharacterRelations)
                .FirstOrDefaultAsync(o => o.Id == claim.GamerId);
            var targetCharacter = await _context.Characters.AsTracking()
                .FirstOrDefaultAsync(o => o.Id == claim.CharacterId);

            if (targetGamer == null && targetCharacter == null || targetGamer.Cash < targetCharacter.Price)
                return new JsonResult(null)
                {
                    StatusCode = BadRequest().StatusCode,
                    Value = "Money hasn't"
                };

            targetGamer.Cash -= targetCharacter.Price;
            var relationObj = new GamerCharacterRelation
            {
                GamerId = targetGamer.Id,
                CharacterId = targetCharacter.Id,
            };

            targetGamer.GamerCharacterRelations.Add(relationObj);

            await _context.GamerCharacterRelations.AddAsync(relationObj);
            await _context.SaveChangesAsync();

            return new JsonResult(targetGamer) {
                StatusCode = Ok().StatusCode
            };
        }

    }
}
