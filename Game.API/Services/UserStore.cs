using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.DAL;
using Infrastructure.DAL.Models;

namespace Game.API.Services
{
    public class OwnUserStore: UserStore<User, Role, ApplicationContext, Guid>
    {
        public OwnUserStore(ApplicationContext context) : base(context)
        {
        }
    }
}
