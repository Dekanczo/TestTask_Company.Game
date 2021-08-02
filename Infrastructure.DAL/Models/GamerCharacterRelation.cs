using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructure.DAL.Models
{
    public partial class GamerCharacterRelation
    {
        public Guid GamerId { get; set; }
        public Guid CharacterId { get; set; }

        public virtual Character Character { get; set; }
        public virtual Gamer Gamer { get; set; }
    }
}
