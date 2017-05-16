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

        public IActionResult NoResult()
        {
            return View();
        }

        public IActionResult DestinationQ(string dest)
        {

            var thisRoute = db.Routes.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId);
            thisRoute.DestinationType = dest;
            db.Entry(thisRoute).State = EntityState.Modified;
            db.SaveChanges();
            if (dest == "D"){
                
                return Json(true);
            } else{

                return Json(false);
            }
        }


        public IActionResult GetLocations(string auto, string radius, string locationType)
        {
                Debug.WriteLine(radius);

                int radiusInt = 0;
                if (radius == null)
                {
                    radiusInt = 16000;
                } else
                {
                    radiusInt = Int32.Parse(radius);
                }

                Debug.WriteLine(radiusInt);

                var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
                var originLatitiude = thisPlace.Latitude;
                var originLongitude = thisPlace.Longitude;
                var allLocations = new List<Location>();
                if(radiusInt > 40000)
                {
                    var allLocationsYelp = YelpPlace.GetLocationsExtend(auto, originLatitiude, originLongitude);
                    var allLocationsGoogle = GoogleAuto.GetGoogleAddress(auto, originLatitiude, originLongitude, radiusInt);
                    allLocations = allLocationsYelp;
                    allLocations.AddRange(allLocationsGoogle);
                } else
                {
                    var allLocationsYelp = YelpPlace.GetLocations(auto, originLatitiude, originLongitude, radiusInt);
                    var allLocationsGoogle = GoogleAuto.GetGoogleAddress(auto, originLatitiude, originLongitude, radiusInt);
                    allLocations = allLocationsYelp;
                    allLocations.AddRange(allLocationsGoogle);
                }
                foreach (Location place in allLocations)
                {
                    place.LocationType = locationType;
                }
            if (allLocations.Count > 0)
                {
                    return PartialView(allLocations);

                }
                else
                {
                    return RedirectToAction("NoResult");
                }

        }

        public IActionResult AddLocation(string name, string address, string city, string state, string zip, string latitude, string longitude, string locationType)
        {
            string thisLocationType = "";
            if(locationType == null)
            {
                thisLocationType = "W";
            } else
            {
                thisLocationType = locationType;
            }
             

            Location newLocation = new Location(name, address, city, state, zip, latitude, longitude, thisLocationType, GlobalRoute.RouteId);
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

        public IActionResult GetAddressAuto(string term)
        {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allAuto = GoogleAuto.GetGoogleAddressAuto(term, originLatitiude, originLongitude);
            return Json(allAuto);
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
