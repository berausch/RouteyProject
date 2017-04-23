using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Routey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Routey.Controllers
{
    public static class GlobalRoute
    {
       public static int RouteId { get; set; }
    }

    public class HomeController : Controller
    {
        

        private ApplicationDbContext db = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetLocations(string auto)
        {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allLocations = YelpPlace.GetLocations(auto, originLatitiude, originLongitude);
            for(var i = 0; i < allLocations.Count; i++)
            {
                allLocations[i].YelpTerm = auto;
                
            }

            return View(allLocations);
        }

        public IActionResult AddLocation(string id)
        {
            var thisPlace = YelpPlace.AddLocation(id);
            string locationType = "W";
         
            thisPlace.RouteId = GlobalRoute.RouteId;
            Location newLocation = new Location(thisPlace.Name, thisPlace.Location.Address1, thisPlace.Location.City, thisPlace.Location.State, thisPlace.Location.Zip_code, thisPlace.Coordinates.Latitude, thisPlace.Coordinates.Longitude, thisPlace.Id, locationType);
            newLocation.apiAddress();

            newLocation.RouteId = GlobalRoute.RouteId;
            db.Locations.Add(newLocation);
            db.SaveChanges();

            return View(thisPlace);
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult GetAuto(string term)
        {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allAuto1 = autoPlace.GetAutocompleteBusinesses(term, originLatitiude, originLongitude);
            var allAuto2 = autoPlace.GetAutocompleteTerms(term, originLatitiude, originLongitude);

            allAuto1.AddRange(allAuto2);
            return Json(allAuto1);
        }

        public IActionResult OriginDestQ(string Origin)
        {
            Debug.WriteLine(Origin);
            return Content(Origin);
        }

        public IActionResult NewRoute()
        {

            Route newRoute = new Route();
            db.Routes.Add(newRoute);
            db.SaveChanges();

            GlobalRoute.RouteId = newRoute.RouteId;
            var checkGlobal = GlobalRoute.RouteId;
            return View(newRoute);
        }

        public IActionResult CreateNewOrigin(string originAddress, string originCity, string originState)
        {

            string locationType = "OD"; 
            var newLocation = GoogleLatLng.GetLatLng(originAddress, originCity, originState);
            newLocation.LocationType = locationType;
            newLocation.RouteId = GlobalRoute.RouteId;
            newLocation.Name = "Origin";

            db.Locations.Add(newLocation);
            db.SaveChanges();
            Debug.WriteLine(newLocation);
            return Json(newLocation);
        }

    }
}
