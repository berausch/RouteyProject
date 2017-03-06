using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Routey.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Routey.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetLocations()
        {
            var allLocations = YelpPlace.GetLocations();
            return View(allLocations);
        }

        public IActionResult AddLocation(int id)
        {
            var allLocations = YelpPlace.GetLocations();

            var thisLocation = allLocations[id];

            return View(thisLocation);
        }
    }
}
