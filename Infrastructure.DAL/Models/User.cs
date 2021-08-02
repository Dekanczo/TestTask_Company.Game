using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.DAL.Models
{
    public partial class User: IdentityUser<Guid>
    {
        public ICollection<Gamer> Gamers { get; set; }
    }
}