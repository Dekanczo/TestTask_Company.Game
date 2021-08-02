 using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.DAL.Models;

namespace Infrastructure.DAL
{
    public class CharacterSeeder: Seeder
    {
        public CharacterSeeder() { }
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.Characters.Any())
            {
                Faker<Character> faker = new Faker<Character>();
                faker
                    .RuleFor(o => o.Id, f => Guid.NewGuid())
                    .RuleFor(o => o.Name, f => Faker.Name.First())
                    .RuleFor(o => o.Price, f => Faker.RandomNumber.Next(0, 200000));

                context.Characters.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
