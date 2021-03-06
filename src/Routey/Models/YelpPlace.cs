﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Routey.Models
{
  
    public class YelpPlace
    {
        public string Name { get; set; }

        public string YelpTerm { get; set; }
        public YelpLocation Location { get; set; }

        public Coordinate Coordinates { get; set; }
        public class YelpLocation
        {
            public string Address1 { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Zip_code { get; set; }

        }

        public class Coordinate
        {
            public string Longitude;
            public string Latitude;
        }
         public string Id { get; set; }
         public int RouteId { get; set; }





        public static List<Location> GetLocations(string userInput, string lat, string lon, int radius)
        {
            var client = new RestClient("https://api.yelp.com/v3/businesses/search?&");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "af248707-3a53-4d59-a269-b10e3a1dffc6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer Drh3Xrl-pkzYk-KyZtWgqtBx_uZHrbCzP7vFAnCdUydCSgdrmP1AV4_1sKKhKIuoKVqLNh9NKb0t2hPIybxt6CB9tqtShtUVguyOadwm4-t_0kI2mQSV5gtcR5O9WHYx");
            request.AddParameter("term", userInput);
            request.AddParameter("radius", radius);
            request.AddParameter("latitude", lat);
            request.AddParameter("longitude", lon);

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var yelplocationList = JsonConvert.DeserializeObject<List<YelpPlace>>(jsonResponse["businesses"].ToString());

            var locationObjectList = yelplocationList.Select(x => new Location(x.Name, x.Location.Address1, x.Location.City, x.Location.State, x.Location.Zip_code, x.Coordinates.Latitude, x.Coordinates.Longitude, x.Id)).ToList();

            return locationObjectList;

        }

        public static List<Location> GetLocationsExtend(string userInput, string lat, string lon)
        {
            var client = new RestClient("https://api.yelp.com/v3/businesses/search?&");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "af248707-3a53-4d59-a269-b10e3a1dffc6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer Drh3Xrl-pkzYk-KyZtWgqtBx_uZHrbCzP7vFAnCdUydCSgdrmP1AV4_1sKKhKIuoKVqLNh9NKb0t2hPIybxt6CB9tqtShtUVguyOadwm4-t_0kI2mQSV5gtcR5O9WHYx");
            request.AddParameter("term", userInput);
            request.AddParameter("latitude", lat);
            request.AddParameter("longitude", lon);

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var yelplocationList = JsonConvert.DeserializeObject<List<YelpPlace>>(jsonResponse["businesses"].ToString());

            var locationObjectList = yelplocationList.Select(x => new Location(x.Name, x.Location.Address1, x.Location.City, x.Location.State, x.Location.Zip_code, x.Coordinates.Latitude, x.Coordinates.Longitude, x.Id)).ToList();

            return locationObjectList;

        }



        public static YelpPlace AddLocation(string yelpId)
        {
            var client = new RestClient("https://api.yelp.com/v3/businesses/" + yelpId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "af248707-3a53-4d59-a269-b10e3a1dffc6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer Drh3Xrl-pkzYk-KyZtWgqtBx_uZHrbCzP7vFAnCdUydCSgdrmP1AV4_1sKKhKIuoKVqLNh9NKb0t2hPIybxt6CB9tqtShtUVguyOadwm4-t_0kI2mQSV5gtcR5O9WHYx");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var locationAdd = JsonConvert.DeserializeObject<YelpPlace>(jsonResponse.ToString());
            return locationAdd;

        }


        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

    }
}
