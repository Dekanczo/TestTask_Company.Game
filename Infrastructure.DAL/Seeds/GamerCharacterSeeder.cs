using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.DAL.Models;

namespace Infrastructure.DAL
{
    public class GamerCharacterSeeder : Seeder
    {
        public GamerCharacterSeeder() { }
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.GamerCharacterRelations.Any())
            {
                var GamerIds = context.Gamers.Select(c => c.Id).ToList();
                var CharacterIds = context.Characters.Select(c => c.Id).ToList();
                Faker<GamerCharacterRelation> faker = new Faker<GamerCharacterRelation>();
                faker
                    .RuleFor(o => o.GamerId, f => GamerIds[f.Random.Int(0, GamerIds.Count - 1)])
                    .RuleFor(o => o.CharacterId, f => CharacterIds[f.Random.Int(0, CharacterIds.Count - 1)]);

                //context.GamerCharacterRelations.;
                context.GamerCharacterRelations.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
