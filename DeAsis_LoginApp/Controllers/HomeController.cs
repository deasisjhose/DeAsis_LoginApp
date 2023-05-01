using DeAsis_LoginApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

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

                    var territoriesWrapper = JsonConvert.DeserializeObject<TerritoriesWrapper>(content);
                    var territories = territoriesWrapper.Data.ToList();
                   

                    Console.WriteLine(content.ToString()); 
                    // Do something with the territories
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
            public Territory[] Data { get; set; }
        }


    }

}