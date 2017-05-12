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

        public IActionResult GetLocations(string auto, string Command)
        {
            Debug.WriteLine(Command);

            if(Command == "submit")
            {
                var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
                var originLatitiude = thisPlace.Latitude;
                var originLongitude = thisPlace.Longitude;
                var allLocationsYelp = YelpPlace.GetLocations(auto, originLatitiude, originLongitude);
                var allLocationsGoogle = GoogleAuto.GetGoogleAddress(auto, originLatitiude, originLongitude);
                var allLocations = allLocationsYelp;
                allLocations.AddRange(allLocationsGoogle);
                Debug.WriteLine(allLocations);
                if (allLocations.Count > 0)
                {
                    return PartialView(allLocations);

                }
                else
                {
                    return RedirectToAction("NoResult");
                }
            } else if(Command == "extend")
            {
                var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
                var originLatitiude = thisPlace.Latitude;
                var originLongitude = thisPlace.Longitude;
                var allLocationsYelp = YelpPlace.GetLocationsExtend(auto, originLatitiude, originLongitude);
                var allLocationsGoogle = GoogleAuto.GetGoogleAddressExtend(auto, originLatitiude, originLongitude);
                var allLocations = allLocationsYelp;
                allLocations.AddRange(allLocationsGoogle);
                Debug.WriteLine(allLocations);
                if (allLocations.Count > 0)
                {
                    return PartialView(allLocations);

                }
                else
                {
                    return RedirectToAction("NoResultExtend");
                }
            }

            return RedirectToAction("Error");


        }

        public IActionResult NoResult()
        {

            return PartialView();
        }

        public IActionResult NoResultExtend()
        {

            return PartialView();
        }

        public IActionResult AddLocation(string name, string address, string city, string state, string zip, string latitude, string longitude)
        {
            
            string locationType = "W";

            Location newLocation = new Location(name, address, city, state, zip, latitude, longitude,locationType, GlobalRoute.RouteId);
            newLocation.apiAddress();
            Debug.WriteLine(newLocation);
            db.Locations.Add(newLocation);
            db.SaveChanges();

            return PartialView(newLocation);
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
                var newLocation = GoogleLatLng.GetLatLng(thisLocation[0].Address, thisLocation[0].City, thisLocation[0].State);
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

        public IActionResult Route()
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
