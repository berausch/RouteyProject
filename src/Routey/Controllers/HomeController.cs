using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Routey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Routey.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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
            var allPlaces = YelpPlace.GetLocations();

            var thisPlace = allPlaces[id];
            Location newLocation = new Location(thisPlace.Name, thisPlace.Location.Address1, thisPlace.Location.City, thisPlace.Location.State, thisPlace.Location.Zip_code, thisPlace.Coordinates.Latitude, thisPlace.Coordinates.Longitude, thisPlace.Id, false);

            db.Locations.Add(newLocation);
            db.SaveChanges();

            return View(thisPlace);
        }
    }
}
