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


                    // Arrange the territories based on the correlation between the fields "id" and "parent"
                    var territoryDictionary = new Dictionary<string, Territory>();
                    foreach (var territory in territories)
                    {
                        territoryDictionary.Add(territory.Id, territory);
                    }

                    var arrangedTerritories = new List<Territory>();
                    foreach (var territory in territories)
                    {
                        if (territory.Parent != null && territoryDictionary.ContainsKey(territory.Parent))
                        {
                            var parent = territoryDictionary[territory.Parent];
                            parent.Children.Add(territory);
                        }
                        else
                        {
                            arrangedTerritories.Add(territory);
                        }
                    }

                    //Sort arranged
                    var sortedTerritories = arrangedTerritories.OrderBy(t => t.Id).ToList();


                    // Do something with the territories
                    foreach(var territory in sortedTerritories)
                    {
                        Console.WriteLine(territory.Id); 
                    }


                    return View(sortedTerritories);
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