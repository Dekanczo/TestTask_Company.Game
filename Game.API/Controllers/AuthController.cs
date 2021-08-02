using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DAL;
using Infrastructure.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Game.API.Models;
using Microsoft.AspNetCore.Authorization;
using Game.API.Configurations;

namespace Game.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AuthController: Controller
    {
        private readonly string _provider = "Standard";
        private readonly AuthService _service;
        
        public AuthController(AuthService service, string provider = null)
        {
            _provider = provider != null ? provider : _provider;
            _service = service;
        }

        [HttpPost("[action]")]
        public virtual async Task<ActionResult<AuthResponseModel>> Register([FromBody] string providerKey)
        {
            var result = await _service.RegisterAsync(_provider, providerKey);

            if (result == null)
                return new JsonResult("Authentication failed: such login already exists")
                {
                    StatusCode = Unauthorized().StatusCode
                };

            return new JsonResult(result)
            {
                StatusCode = Ok().StatusCode
            };
        } 

        [HttpPost("[action]")]
        public virtual async Task<ActionResult<AuthResponseModel>> Login([FromBody] string providerKey)
        {
            var alreadySigIn = _service.SignInManager.IsSignedIn(User);

            AuthResponseModel result = 
                alreadySigIn
                    ? await _service.RefreshSignInAsync(_provider, providerKey)
                    : await _service.SignInAsync(_provider, providerKey);


            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", result.Token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(AuthConfiguration.LIFETIME)
            });
            HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            HttpContext.Response.Headers.Add("X-Xss-Protection", "1");
            HttpContext.Response.Headers.Add("X-Frame-Options", "DENY");


            return new JsonResult(result)
            {
                StatusCode = Ok().StatusCode
            };
        }


        [HttpPost("[action]")]
        public virtual async Task LogOff()
            => await _service.SignOutAsync();

    }
}
