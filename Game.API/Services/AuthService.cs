using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using Infrastructure.DAL;
using Infrastructure.DAL.Models;
using Game.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Game.API.Configurations;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Game.API
{
    public class AuthService
    {

        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public SignInManager<User> SignInManager => _signInManager;

        public AuthService(ApplicationContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthResponseModel> RegisterAsync(string provider, string providerKey)
        {
            var exist = await _userManager.FindByLoginAsync(provider, providerKey);

            if (exist != null)
                return null;

            var guid = Guid.NewGuid();

            var user = new User
            {
                Id = guid,
                UserName = guid.ToString()
            };

            var registerResult = await _userManager.CreateAsync(user);
            if (!registerResult.Succeeded)
                throw new Exception(String.Join("\n", registerResult.Errors));
            
            var login = new UserLoginInfo(provider, providerKey, string.Empty);
            await _userManager.AddLoginAsync(user, login);

            await _context.SaveChangesAsync();

            var response = new AuthResponseModel(user.Id);

            return response;
        }

        public async Task<AuthResponseModel> SignInAsync(string provider, string providerKey)
        {
            var user = await _userManager.FindByLoginAsync(provider, providerKey)
                ?? throw new Exception("User does not exist");

            var wasLogedIn = await _signInManager.CanSignInAsync(user);
            if (!wasLogedIn)
                throw new Exception("Can not sign in");

            await _signInManager.SignInAsync(user, true);

            var gamer = await _context.Gamers.FirstOrDefaultAsync(p => p.UserId == user.Id);

            var token = GenerateToken(user.Id);

            await _context.SaveChangesAsync();

            return new AuthResponseModel(user.Id, token);
        }


        public async Task<AuthResponseModel> RefreshSignInAsync(string provider, string providerKey)
        {
            var user = await _userManager.FindByLoginAsync(provider, providerKey)
                ?? throw new Exception("User does not exist");


            var gamer = await _context.Gamers.FirstOrDefaultAsync(p => p.UserId == user.Id);
            var token = GenerateToken(user.Id);

            await _signInManager.RefreshSignInAsync(user);
            await _context.SaveChangesAsync();

            return new AuthResponseModel(user.Id, token);
        }

        public async Task SignOutAsync()
            => await _signInManager.SignOutAsync();

        private string GenerateToken(Guid userId)
        {
            var identity = GetIdentity(userId);
            if (identity == null)
                throw new Exception("Invalid auth data");

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthConfiguration.ISSUER,
                    audience: AuthConfiguration.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthConfiguration.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private static ClaimsIdentity GetIdentity(Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
            };

            return new ClaimsIdentity(
                claims, 
                "Token", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
        }
    }
}
