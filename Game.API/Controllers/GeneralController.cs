using Microsoft.AspNetCore.Mvc;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Game.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        protected readonly ApplicationContext _context;

        public GeneralController(ApplicationContext context)
        {
            _context = context;
        }
    }
}
