using System;
using System.Linq;
using Infrastructure.DAL.Models;
using Bogus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DAL
{
    public class GamerSeeder: Seeder
    {
        private int _counter = 0;
        public int Counter {
            get {
                return _counter++;
            }
        }


        public GamerSeeder() {}
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.Gamers.Any())
            {
                var users = context.Users.ToArray();

                Faker<Gamer> faker = new Faker<Gamer>();

                faker
                    .RuleFor(o => o.Id, f => Guid.NewGuid())
                    .RuleFor(o => o.Name, f => Faker.Name.First())
                    .RuleFor(o => o.Level, f => Faker.RandomNumber.Next(200000))
                    .RuleFor(o => o.Cash, f => Faker.RandomNumber.Next(100000))
                    .RuleFor(o => o.UserId, f => users[Counter].Id);

                context.Gamers.AddRange(faker.Generate(MaxObjectNumber));

                context.SaveChanges();
            }
        }

    }
}
