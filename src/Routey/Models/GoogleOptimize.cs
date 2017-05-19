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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Routey.Models
{
    public class GoogleOptimize
    {
      
        public List<int> Waypoint_order { get; set; }

        public static List<int> GetGoogleOrder(Location origin, Location destination, List<Location> waypoints)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/directions/json?");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("origin", origin.AddressConcat);
            request.AddParameter("destination", destination.AddressConcat);
            string allWaypoints = "optimize:true|";
            for(var i = 0; i < waypoints.Count; i++)
            {
                 if(i < (waypoints.Count-1))
                {
                    allWaypoints = allWaypoints + waypoints[i].AddressConcat + "|";
                } else
                {
                    allWaypoints = allWaypoints + waypoints[i].AddressConcat;
                }
            }

            request.AddParameter("waypoints", allWaypoints);
            request.AddParameter("language", "en");

            request.AddParameter("key", "AIzaSyBniQDIBB4eoG7DLjs29N0Hm2bZRiJJrVA");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var optimizeList = JsonConvert.DeserializeObject<List<GoogleOptimize>>(jsonResponse["routes"].ToString());
            List<int> thisLocationList = optimizeList[0].Waypoint_order;

            return thisLocationList;
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
