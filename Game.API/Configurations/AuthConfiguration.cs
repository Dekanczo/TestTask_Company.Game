using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Faker;

namespace Game.API.Configurations
{
    public class AuthConfiguration
    {
        public const string ISSUER = "OwnServer";
        public const string AUDIENCE = "OwnWebClient";
        public static readonly string KEY = (new Bogus.Faker()).Random.String(32);
        public const int LIFETIME = 24*60; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
