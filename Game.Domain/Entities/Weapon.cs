using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities
{
    [Table("Weapons")]
    public class Weapon
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Damage { get; set; }
        public double Speed { get; set; }
        public double Cooldown { get; set; }
        public int MagazineVolume { get; set; }
        public int Level { get; set; }

        [NotMapped]
        public ICollection<Character> Characters { get; set; }
    }
}
