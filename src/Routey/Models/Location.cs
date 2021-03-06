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
        public string AddressConcat { get; set; }
        public string NameConcat { get; set; }

        public string GoogleId { get; set; }
        public string AddressDisplay { get; set; }

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
        public Location(string address, string city, string state, int routeId, string locationType, bool save = false)
        {
            Address = address;
            City = city;
            State = state;
            RouteId = routeId;
            LocationType = locationType;
            Save = save;
        }
        public Location(string address, string city, string state)
        {
            Address = address;
            City = city;
            State = state;
        }

        public Location(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(string name, string address, string city, string state, string googleId)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            GoogleId = googleId;
        }



        public Location(string name, string address, string city, string state, string zip, string latitude, string longitude, string yelpId, string locationType, bool save = false)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Latitude = latitude;
            Longitude = longitude;           
            YelpId = yelpId;
            LocationType = locationType;
            Save = save;
        }

        public Location(string name, string address, string city, string state, string zip, string latitude, string longitude, string yelpId)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Latitude = latitude;
            Longitude = longitude;
            YelpId = yelpId;
        }

        public Location(string name, string address, string city, string state, string zip, string latitude, string longitude, string locationType, int routeId)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Latitude = latitude;
            Longitude = longitude;
            LocationType = locationType;
            RouteId = routeId;
        }

        public void ApiAddress()
        {
            string addressNoSpace = Address.Replace(" ", "+");
            string cityNoSpace = City.Replace(" ", "+");
            string stateNoSpace = State.Replace(" ", "+");

            string allAddress = addressNoSpace + "+" + cityNoSpace + "+" + stateNoSpace;

           AddressConcat = allAddress;
           
           
        }

        public void ApiName()
        {
            string nameNoSpace = Name.Replace(" ", "+");
            NameConcat = nameNoSpace;

        }


    }



}
