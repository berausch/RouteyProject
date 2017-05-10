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
            var allLocationsYelp = YelpPlace.GetLocations(auto, originLatitiude, originLongitude);
            var allLocationsGoogle = GoogleAuto.GetGoogleAddress(auto, originLatitiude, originLongitude);

            var allLocations = allLocationsYelp;
            allLocations.Add(allLocationsGoogle);

            return PartialView(allLocations);
        }

        public IActionResult AddLocation(Location thisPlace)
        {
            string locationType = "W";
         
            thisPlace.RouteId = GlobalRoute.RouteId;
            thisPlace.LocationType = locationType;
            thisPlace.apiAddress();

            thisPlace.RouteId = GlobalRoute.RouteId;
            db.Locations.Add(thisPlace);
            db.SaveChanges();

            return PartialView(thisPlace);
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
            var allAutoBiz = YelpAuto.GetAutocompleteBusinesses(term, originLatitiude, originLongitude);
            var allAutoTerm = YelpAuto.GetAutocompleteTerms(term, originLatitiude, originLongitude);
            var allAutoAddress = GoogleAuto.GetGoogleAddressAuto(term, originLatitiude, originLongitude);

            var allAuto = allAutoBiz;
            allAuto.AddRange(allAutoTerm);
            allAuto.AddRange(allAutoAddress);
            return Json(allAuto);
        }

        public IActionResult GetGoogleAuto(string term)
        {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allAuto = GoogleAuto.GetGoogleAddressAuto(term, originLatitiude, originLongitude);
            return Json(allAuto);
        }


        public IActionResult SetGoogleAddress(string auto)
        {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var Check = GoogleAuto.GetGoogleAddressAuto(auto, originLatitiude, originLongitude);
            if (Check.Count > 0)
            {
                var thisLocation = GoogleAuto.GetGoogleAddress(auto, originLatitiude, originLongitude);
                var newLocation = GoogleLatLng.GetLatLng(thisLocation.Address, thisLocation.City, thisLocation.State);
                newLocation.RouteId = GlobalRoute.RouteId;
                newLocation.LocationType = "W";

                db.Locations.Add(newLocation);
                db.SaveChanges();
                return Json(newLocation);
            }
            else {
                bool noSubmit = false;

                return Json(noSubmit);
            } 
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

        public IActionResult EndRoute()
        {
            var mapLink = "https://www.google.com/maps/dir";
            
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            mapLink = mapLink + "/" + thisPlace.AddressConcat;
            return Json(mapLink);
        }

    }
}
