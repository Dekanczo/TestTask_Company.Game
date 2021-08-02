using System;
using System.Linq;
using Infrastructure.DAL.Models;
using Bogus;

namespace Infrastructure.DAL
{
    public class WeaponSeeder: Seeder
    {
        public WeaponSeeder() { }
        public void Initialize()
        {
            ApplicationContext context = new ApplicationContext();

            if (!context.Weapons.Any())
            {
                Faker<Weapon> faker = new Faker<Weapon>();
                faker
                    .RuleFor(o => o.Id, f => Guid.NewGuid())
                    .RuleFor(o => o.Name, f => Faker.Name.First())
                    .RuleFor(o => o.Price, f => Faker.RandomNumber.Next(0, 200000))
                    .RuleFor(o => o.Damage, f => Faker.RandomNumber.Next(0, 100000))
                    .RuleFor(o => o.Speed, f => Faker.RandomNumber.Next(0, 500))
                    .RuleFor(o => o.Cooldown, f => Faker.RandomNumber.Next(0, 300))
                    .RuleFor(o => o.MagazineVolume, f => Faker.RandomNumber.Next(0, 500))
                    .RuleFor(o => o.Level, f => Faker.RandomNumber.Next(0, 100));
                
                context.Weapons.AddRange(faker.Generate(MaxObjectNumber));
                context.SaveChanges();
            }
        }

    }
}
