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
    public class GoogleAuto
    {
        public string Description { get; set; }

        public Address1 structured_formatting { get; set; }

        public string place_id { get; set; }
        public class Address1
        {
            public string main_text { get; set; }
            public string secondary_text { get; set; }
        }

        public string City { get; set; }
        public string State { get; set; }


        public static Location GetGoogleAddress(string userInput, string lat, string lon)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/place/autocomplete/json?");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "d35742c7-3445-607f-20ec-1a1ec565cc0b");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("input", userInput);
            request.AddParameter("location", lat+","+lon);

            request.AddParameter("key", "AIzaSyBniQDIBB4eoG7DLjs29N0Hm2bZRiJJrVA");


            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var autoGoogleList = JsonConvert.DeserializeObject<List<GoogleAuto>>(jsonResponse["predictions"].ToString());

            var secondLine = autoGoogleList[0].structured_formatting.secondary_text.Split(',');
            Location thisLocation = new Location(autoGoogleList[0].structured_formatting.main_text, secondLine[0], secondLine[1].Remove(0, 1), autoGoogleList[0].place_id);

            return thisLocation;
        }

        public static List<string> GetGoogleAddressAuto(string userInput, string lat, string lon)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/place/autocomplete/json?&strictbounds");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "d35742c7-3445-607f-20ec-1a1ec565cc0b");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("input", userInput);
            request.AddParameter("location", lat + "," + lon);
            request.AddParameter("radius", 100000);
            request.AddParameter("types", "address");
            request.AddParameter("key", "AIzaSyBniQDIBB4eoG7DLjs29N0Hm2bZRiJJrVA");


            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var autoGoogleList = JsonConvert.DeserializeObject<List<GoogleAuto>>(jsonResponse["predictions"].ToString());

            List<string> autoList = autoGoogleList.Select(s => s.Description).ToList();

            Debug.WriteLine(response);
            return autoList;
        }
        public static List<string> GetGoogleAddressAutoExtend(string userInput, string lat, string lon)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/place/autocomplete/json?");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "d35742c7-3445-607f-20ec-1a1ec565cc0b");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("input", userInput);
            request.AddParameter("location", lat + "," + lon);
            request.AddParameter("radius", 50);
            request.AddParameter("types", "address");
            request.AddParameter("key", "AIzaSyBniQDIBB4eoG7DLjs29N0Hm2bZRiJJrVA");


            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var autoGoogleList = JsonConvert.DeserializeObject<List<GoogleAuto>>(jsonResponse["predictions"].ToString());

            List<string> autoList = autoGoogleList.Select(s => s.Description).ToList();

            Debug.WriteLine(response);
            return autoList;
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
