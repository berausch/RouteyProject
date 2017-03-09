﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Routey.Models
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }
        public string Zip { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string YelpId { get; set; }

        public bool Save { get; set; }

        public int RouteId { get; set; }

        //Location Types: 
        //  W - Waypoint
        //  O - Origin only
        //  D - Destination only
        //  OD - Origin is the destination as well

        public string LocationType { get; set; }

        public virtual Route Route { get; set; }

        public Location()
        {

        }
        public Location(string address, string city, string state, int routeId, string locationType)
        {
            Address = address;
            City = city;
            State = state;
            RouteId = routeId;
            LocationType = locationType;
        }

        public Location(string name, string address, string city, string state, string zip, string latitude, string longitude, string yelpId, bool save)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Latitude = latitude;
            Longitude = longitude;
            YelpId = yelpId;
            Save = save;

        }

    }



}
