using DeAsis_LoginApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeAsis_LoginApp.Controllers
{
    public class HomeController : Controller
    {

        [Authorize]
        public IActionResult Index()
        {
            Console.WriteLine("NASA HOME NA");
            Console.WriteLine(User.Identity.IsAuthenticated); 
            return View();
        }

       
    }
}