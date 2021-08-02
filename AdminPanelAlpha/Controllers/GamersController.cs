using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Game.AdminPanelAlpha.Models;
using Infrastructure.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Game.AdminPanelAlpha.Controllers
{
    public class GamersController : Controller
    {
        HttpClient _httpClient;

        public GamersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var result = await _httpClient.GetAsync("Gamers");

            var content = JsonConvert.DeserializeObject<IEnumerable<Gamer>>(
                await result.Content.ReadAsStringAsync()
            );

            return View(content);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] Gamer gamer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(gamer), Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await _httpClient.PostAsync("Gamers", content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var authData = JsonConvert.DeserializeObject<Gamer>(await result.Content.ReadAsStringAsync());
                ViewData["ActionMessage"] = "Gamer was added!";
                return View();
            }
            else
            {
                ViewData["ActionMessage"] = "Some error";
                return View();
            }
        }

    }
}
