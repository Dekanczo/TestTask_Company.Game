using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Game.API.Models
{
    public class AuthResponseModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }

        public AuthResponseModel(Guid userId, string token = null)
        {
            UserId = userId;
            Token = token;
        }

    }
}
