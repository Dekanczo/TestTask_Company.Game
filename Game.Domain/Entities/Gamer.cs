using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities
{
    public class Gamer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public double Cash { get; set; }
        [NotMapped]
        public ICollection<Character> Characters { get; set; }
        [NotMapped]
        public ICollection<Weapon> Weapons { get; set; }

    }
}
