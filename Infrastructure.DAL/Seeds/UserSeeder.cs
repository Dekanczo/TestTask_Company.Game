using System;
using System.Linq;
using Infrastructure.DAL.Models;
using Bogus;

namespace Infrastructure.DAL
{
    public class UserSeeder: Seeder
    {
        public UserSeeder() { }
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.Users.Any())
            {
                Faker<User> faker = new Faker<User>();
                faker
                    .RuleFor(o => o.Id, f => Guid.NewGuid())
                    .RuleFor(o => o.Email, f => f.Internet.Email())
                    .RuleFor(o => o.PasswordHash, f => f.Random.String(20))
                    .RuleFor(o => o.UserName, f => f.Name.Random.String(20));

                context.Users.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
