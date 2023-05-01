using DeAsis_LoginApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace DeAsis_LoginApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://netzwelt-devtest.azurewebsites.net/Territories/All");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var jsonArray = JsonConvert.DeserializeObject<TerritoriesWrapper>(content);
                    var territories = jsonArray.Territories;


                    // Do something with the territories
                    Console.WriteLine(territories[0].Name);


                    return View();
                }
                else
                {
                    Console.WriteLine("Failed to retrieve territories");
                    return View();
                }
            }


        }

        public class TerritoriesWrapper
        {
           public TerritoriesWrapper()
            {
                Territories = new List<Territory>(); 
            }
            [JsonProperty("data")]
            public List<Territory> Territories { get; set; }
        }
    }    

}