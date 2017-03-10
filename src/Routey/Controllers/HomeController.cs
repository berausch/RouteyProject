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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetLocations(string auto, int routeId)
        {
            Debug.WriteLine(auto);
            var allLocations = YelpPlace.GetLocations(auto);
            for(var i = 0; i < allLocations.Count; i++)
            {
                allLocations[i].YelpTerm = auto;
                allLocations[i].RouteId = routeId;
            }

            return View(allLocations);
        }

        public IActionResult AddLocation(int id, string term, int routeId)
        {
            var allPlaces = YelpPlace.GetLocations(term);
            string locationType = "W";

            var thisPlace = allPlaces[id];
            Location newLocation = new Location(thisPlace.Name, thisPlace.Location.Address1, thisPlace.Location.City, thisPlace.Location.State, thisPlace.Location.Zip_code, thisPlace.Coordinates.Latitude, thisPlace.Coordinates.Longitude, thisPlace.Id, locationType);
            newLocation.apiAddress();

            newLocation.RouteId = routeId;
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
            var allAuto1 = autoPlace.GetAutocompleteBusinesses(term);
            var allAuto2 = autoPlace.GetAutocompleteTerms(term);

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
            return View(newRoute);
        }

        public IActionResult CreateNewOrigin(string originAddress, string originCity, string originState, int routeId)
        {


            string locationType = "OD";

            Location newLocation = new Location(originAddress, originCity, originState, routeId, locationType);
            Debug.WriteLine(newLocation);
            var thisGoogleLatLng = GoogleLatLng.GetLatLng(originAddress, originCity, originState);
            newLocation.Latitude = thisGoogleLatLng.Latitude;
            newLocation.Longitude = thisGoogleLatLng.Longitude;
            newLocation.AddressConcat = thisGoogleLatLng.AddressConcat;
            newLocation.Name = "Origin";

            db.Locations.Add(newLocation);
            db.SaveChanges();
            Debug.WriteLine(newLocation);
            return Json(newLocation);

        }

        }
    }
