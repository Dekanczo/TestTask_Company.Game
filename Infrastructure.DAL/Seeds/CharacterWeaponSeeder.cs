using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DAL
{
    public class CharacterWeaponSeeder: Seeder
    {
        public CharacterWeaponSeeder() { }
        public void Initialize()
        {
            

            ApplicationContext context = new ApplicationContext();

            if (!context.CharacterWeaponRelations.AsNoTracking().Any())
            {
                var CharacterIds = context.Characters.Select(c => c.Id).ToList();
                var WeaponIds = context.Weapons.Select(c => c.Id).ToList();
                Faker<CharacterWeaponRelation> faker = new Faker<CharacterWeaponRelation>();
                faker
                    .RuleFor(o => o.CharacterId, f => CharacterIds[f.Random.Int(0, CharacterIds.Count - 1)])
                    .RuleFor(o => o.WeaponId, f => WeaponIds[f.Random.Int(0, WeaponIds.Count - 1)]);

                context.CharacterWeaponRelations.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
