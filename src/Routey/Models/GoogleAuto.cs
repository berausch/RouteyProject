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


        public class Address1
        {
            public string main_text { get; set; }
            public string secondary_text { get; set; }
        }

        public static List<string> GetGoogleAddress(string userInput, string lat, string lon)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/place/autocomplete/json?input=2315");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "d35742c7-3445-607f-20ec-1a1ec565cc0b");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("input", userInput);
            request.AddParameter("location", lat+","+lon);

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var autoGoogleList = JsonConvert.DeserializeObject<List<string>>(jsonResponse["predictions"].ToString());



            return autoGoogleList;
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
