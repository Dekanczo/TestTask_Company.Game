using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class Character
    {
        public Character()
        {
            CharacterWeaponRelations = new HashSet<CharacterWeaponRelation>();
            GamerCharacterRelations = new HashSet<GamerCharacterRelation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public virtual ICollection<CharacterWeaponRelation> CharacterWeaponRelations { get; set; }
        public virtual ICollection<GamerCharacterRelation> GamerCharacterRelations { get; set; }
    }
}
