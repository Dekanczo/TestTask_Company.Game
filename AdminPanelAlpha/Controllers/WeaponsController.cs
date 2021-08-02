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

namespace Game.AdminPanelAlpha.Controllers
{
    public class WeaponsController : Controller
    {
        HttpClient _httpClient;

        public WeaponsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var result = await _httpClient.GetAsync("Weapons");

            var content = JsonConvert.DeserializeObject<IEnumerable<Weapon>>(
                await result.Content.ReadAsStringAsync()
            );

            return View(content);
        }

    }
}
