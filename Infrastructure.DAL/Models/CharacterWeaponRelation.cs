using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class CharacterWeaponRelation
    {
        public Guid CharacterId { get; set; }
        public Guid WeaponId { get; set; }

        public virtual Character Character { get; set; }
        public virtual Weapon Weapon { get; set; }
    }
}
