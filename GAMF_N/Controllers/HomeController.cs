using GAMF_N.Models;
using GAMF_N.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GAMF_N.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly GAMFDbContext _Context;
        public HomeController(GAMF_N.Data.GAMFDbContext context)
        {
            _Context = context;
        }

        public IActionResult Index()
        {
            var students = _Context.Students.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}