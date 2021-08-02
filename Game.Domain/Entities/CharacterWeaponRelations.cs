using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities
{
    [Table("CharacterWeaponRelations")]
    public class CharacterWeaponRelation
    {
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public Guid WeaponId { get; set; }
        public Weapon Weapon { get; set; }
    }
}
