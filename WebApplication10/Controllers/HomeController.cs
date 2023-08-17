using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DHBS_Sistemi.Models;

namespace DHBS_Sistemi.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "KimlikDoğrulama");
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Login", "KimlikDoğrulama");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("Token", "");
            return RedirectToAction("Login", "KimlikDoğrulama");
        }
    }
}