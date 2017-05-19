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
            if (dest == "D") {

                return Json(true);
            } else {

                return Json(false);
            }
        }


        public IActionResult GetLocations(string auto, string radius)
        {
            var locationType = "W";
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

            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD" || p.LocationType == "O");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allLocations = new List<Location>();
            if (radiusInt > 40000)
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

        public IActionResult GetLocationsDest(string autoDest, string radiusDest)
        {

            Debug.WriteLine(radiusDest);
            var locationType = "D";
            int radiusInt = 0;
            if (radiusDest == null)
            {
                radiusInt = 16000;
            }
            else
            {
                radiusInt = Int32.Parse(radiusDest);
            }

            Debug.WriteLine(radiusInt);

            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD" || p.LocationType == "O");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allLocations = new List<Location>();
            if (radiusInt > 40000)
            {
                var allLocationsYelp = YelpPlace.GetLocationsExtend(autoDest, originLatitiude, originLongitude);
                var allLocationsGoogle = GoogleAuto.GetGoogleAddress(autoDest, originLatitiude, originLongitude, radiusInt);
                allLocations = allLocationsYelp;
                allLocations.AddRange(allLocationsGoogle);
            }
            else
            {
                var allLocationsYelp = YelpPlace.GetLocations(autoDest, originLatitiude, originLongitude, radiusInt);
                var allLocationsGoogle = GoogleAuto.GetGoogleAddress(autoDest, originLatitiude, originLongitude, radiusInt);
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
            if(thisLocationType == "D")
            {
                if(name == null)
                {
                    name = "Destination";
                }
                var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
                thisPlace.LocationType = "O";
                db.Entry(thisPlace).State = EntityState.Modified;
                db.SaveChanges();
            }
             

            Location newLocation = new Location(name, address, city, state, zip, latitude, longitude, thisLocationType, GlobalRoute.RouteId);
            newLocation.apiAddress();
            newLocation.apiName();
            Debug.WriteLine(newLocation);
            db.Locations.Add(newLocation);
            db.SaveChanges();

 

            return PartialView(newLocation);
        }


        public IActionResult GetAuto(string term)
       {
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD" || p.LocationType == "O");
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
            var thisPlace = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD" || p.LocationType == "O");
            var originLatitiude = thisPlace.Latitude;
            var originLongitude = thisPlace.Longitude;
            var allAuto = GoogleAuto.GetGoogleAddressAuto(term, originLatitiude, originLongitude);
            return Json(allAuto);
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

            if(newLocation == null)
            {
                return Json(false);
            } else
            {
                newLocation.LocationType = locationType;
                newLocation.RouteId = GlobalRoute.RouteId;
                newLocation.Name = "Origin";
                newLocation.apiName();
                db.SaveChanges();

                db.Locations.Add(newLocation);
                db.SaveChanges();
                Debug.WriteLine(newLocation);
                return Json(newLocation);
            }
            
        }

        public IActionResult EndRoute()
        {
            var mapLink = "https://www.google.com/maps/dir";
            var thisRoute = db.Routes.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId);
            var origin = new Location();
            var destination = new Location();
            var waypoints = new List<Location>();
            if (thisRoute.DestinationType == "OD" || thisRoute.DestinationType == "W")
            {
                origin = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "OD");
                destination = origin;
                waypoints = db.Locations.Where(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "W").ToList();
            } else
            {
                origin = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "O");
                destination = db.Locations.FirstOrDefault(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "D");
                waypoints = db.Locations.Where(p => p.RouteId == GlobalRoute.RouteId && p.LocationType == "W").ToList();
            }

            var optimizedOrder = GoogleOptimize.GetGoogleOrder(origin, destination, waypoints);
            var waypointsString = "";
            for(var i=0; i < optimizedOrder.Count; i++)
            {
                if(i < (optimizedOrder.Count -1))
                {
                    waypointsString = waypointsString + waypoints[optimizedOrder[i]].NameConcat + "+" +waypoints[optimizedOrder[i]].AddressConcat + "/";
                }
                else
                {
                    waypointsString = waypointsString + waypoints[optimizedOrder[i]].AddressConcat;
                }
                
            }

            if(thisRoute.DestinationType == "W")
            {
                mapLink = mapLink + "/" + origin.AddressConcat + "/" + waypointsString;
            } else
            {
                mapLink = mapLink + "/" + origin.AddressConcat + "/" + waypointsString + "/"+destination.AddressConcat;
            }
            
            return Json(mapLink);
        }


    }
}
