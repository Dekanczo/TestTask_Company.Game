using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Infrastructure.DAL.Models;
using System.Collections.Generic;
using System;

namespace Infrastructure.DAL
{
    public class Seeder
    {
        public static readonly int MaxObjectNumber = 5;
        
        public static void Start()
        {
            (new UserSeeder()).Initialize();
            (new WeaponSeeder()).Initialize();
            (new CharacterSeeder()).Initialize();
            (new CharacterWeaponSeeder()).Initialize();
            (new GamerSeeder()).Initialize();
            (new GamerWeaponSeeder()).Initialize();
            (new GamerCharacterSeeder()).Initialize();
        }

    }
}
