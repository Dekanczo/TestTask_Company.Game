using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Game.AdminPanelAlpha.Services
{
    public class AuthOptions : AuthenticationSchemeOptions { }

    public class AuthHandler : AuthenticationHandler<AuthOptions>
    {
        public const string SchemeName = "OutsideAPIScheme";
        private readonly HttpClient _httpclient;

        public AuthHandler(
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            HttpClient httpclient)
            : base(options, logger, encoder, clock)
        {
            _httpclient = httpclient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey(".AspNetCore.Application.Token"))
                return AuthenticateResult.Fail("Unauthorized");

            string token = Request.Cookies[".AspNetCore.Application.Token"];

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            try
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", Request.Cookies[".AspNetCore.Application.UserId"]),
                    new Claim("Token", token)
                }, SchemeName);
                ClaimsPrincipal claims = new ClaimsPrincipal(claimsIdentity);
                AuthenticationTicket ticket = new AuthenticationTicket(claims, SchemeName);
                
                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
    }
}
