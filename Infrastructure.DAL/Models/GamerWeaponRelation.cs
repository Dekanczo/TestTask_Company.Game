using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class GamerWeaponRelation
    {
        public Guid GamerId { get; set; }
        public Guid WeaponId { get; set; }

        public virtual Gamer Gamer { get; set; }
        public virtual Weapon Weapon { get; set; }
    }
}
