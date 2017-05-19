using System;
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
    public class GoogleLatLng
    {
        public GoogleGeometry Geometry { get; set; }

        
        public class GoogleGeometry
        {
            public GoogleLocation Location { get; set; }
            public class GoogleLocation
            {
                public string Lat { get; set; }
                public string Lng { get; set; }
            }
        }


        public static Location GetLatLng(string address, string city, string state)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/geocode/json?");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "d9fafe71-44a8-19f1-e338-3f7af10e97ca");
            request.AddHeader("cache-control", "no-cache");

            Location thisLocation = new Location(address, city, state);
            thisLocation.ApiAddress();
            request.AddParameter("address", thisLocation.AddressConcat);

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var newLatLng = JsonConvert.DeserializeObject<List<GoogleLatLng>>(jsonResponse["results"].ToString());

            var thisLat = newLatLng[0].Geometry.Location.Lat.ToString();
            var thisLng = newLatLng[0].Geometry.Location.Lng.ToString();

            thisLocation.Latitude = thisLat;
            thisLocation.Longitude = thisLng;
            Debug.WriteLine(thisLocation);

            return thisLocation;
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
