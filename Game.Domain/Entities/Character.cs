using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        [NotMapped]
        public ICollection<CharacterWeaponRelation> Weapons { get; set; } = new List<CharacterWeaponRelation>();
    }
}
