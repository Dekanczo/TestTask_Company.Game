using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Game.API.Models
{
    public class CharacterBuyingClaim
    {
        public Guid GamerId { get; set; }
        public Guid CharacterId { get; set; }
    }
}
