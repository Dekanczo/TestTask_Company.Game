using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Game.AdminPanelAlpha.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

namespace Game.AdminPanelAlpha.Controllers
{
    public class AuthController : Controller
    {
        HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.UserId");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Token");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login)
        {
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await _httpClient.PostAsync("Login", content);


            if (result.StatusCode == HttpStatusCode.OK)
            {
                var authData = JsonConvert.DeserializeObject<AuthDataModel>(await result.Content.ReadAsStringAsync());
                HttpContext.Response.Cookies.Append(
                    ".AspNetCore.Application.Token",
                    authData.Token,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromMinutes(24 * 60)
                    }
                );
                HttpContext.Response.Cookies.Append(
                    ".AspNetCore.Application.UserId",
                    authData.UserId,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromMinutes(24 * 60)
                    }
                );

                HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                HttpContext.Response.Headers.Add("X-Xss-Protection", "1");
                HttpContext.Response.Headers.Add("X-Frame-Options", "DENY");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["ActionMessage"] = "Such login does not exists.";
                return View();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Register(string login)
        {
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await _httpClient.PostAsync("Register", content);
            

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var authData = JsonConvert.DeserializeObject<AuthDataModel>(await result.Content.ReadAsStringAsync());
                ViewData["ActionMessage"] = "Sign up successed!";
                return View();
            }
            else
            {
                ViewData["ActionMessage"] = "Such login already exist! Please try again.";
                return View();
            }
        }

    }
}
