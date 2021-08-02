//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class Gamer
    {
        public Gamer()
        {
            GamerCharacterRelations = new HashSet<GamerCharacterRelation>();
            GamerWeaponRelations = new HashSet<GamerWeaponRelation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public float Cash { get; set; }

        [JsonIgnore]
        public virtual ICollection<GamerCharacterRelation> GamerCharacterRelations { get; set; }
        [JsonIgnore]
        public virtual ICollection<GamerWeaponRelation> GamerWeaponRelations { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
