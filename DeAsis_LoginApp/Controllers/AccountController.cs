using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DeAsis_LoginApp.Models;
using System.Text;

namespace DeAsis_LoginApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Console.WriteLine("Login u = " + model.Username + " p = " + model.Password);

            // Call the API to validate the credentials
            using (var httpClient = new HttpClient())
            {
                var credentials = new { username = model.Username, password = model.Password };
                var json = JsonConvert.SerializeObject(credentials);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://netzwelt-devtest.azurewebsites.net/Account/SignIn", data);

                Console.WriteLine(response.IsSuccessStatusCode.ToString());


                if (response.IsSuccessStatusCode)
                {

                    // Create the identity for the authenticated user
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.Username));

                    Console.WriteLine(identity.Name); 
                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


                    Console.WriteLine("moving to home");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Display an error message if the credentials are invalid
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                    return View(model);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Remove the authentication cookie and redirect to the Login action
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
