using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.DAL.Models;

namespace Infrastructure.DAL
{
    public class GamerWeaponSeeder : Seeder
    {
        public GamerWeaponSeeder() { }
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.GamerWeaponRelations.Any())
            {
                var GamerIds = context.Gamers.Select(c => c.Id).ToList();
                var WeaponIds = context.Weapons.Select(c => c.Id).ToList();
                Faker<GamerWeaponRelation> faker = new Faker<GamerWeaponRelation>();
                faker
                    .RuleFor(o => o.GamerId, f => GamerIds[f.Random.Int(0, GamerIds.Count - 1)])
                    .RuleFor(o => o.WeaponId, f => WeaponIds[f.Random.Int(0, WeaponIds.Count - 1)]);

                //context.GamerWeaponRelations.;
                context.GamerWeaponRelations.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
