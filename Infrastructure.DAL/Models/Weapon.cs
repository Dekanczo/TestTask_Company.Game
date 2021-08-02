using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class Weapon
    {
        public Weapon()
        {
            CharacterWeaponRelations = new HashSet<CharacterWeaponRelation>();
            GamerWeaponRelations = new HashSet<GamerWeaponRelation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public float Cooldown { get; set; }
        public int MagazineVolume { get; set; }
        public int Level { get; set; }

        public virtual ICollection<CharacterWeaponRelation> CharacterWeaponRelations { get; set; }
        public virtual ICollection<GamerWeaponRelation> GamerWeaponRelations { get; set; }
    }
}
