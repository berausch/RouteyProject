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

namespace Routey.Models
{
  
    public class Place
    {
        public string Name { get; set; }
        public Locations Location { get; set; }
        public class Locations
        {
            public string Address1 { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Zip_code { get; set; }

        }





        public static List<Place> GetLocations()
        {
            var client = new RestClient("https://api.yelp.com/v3/businesses/search?term=BestBuy&location=Portland");
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
            var locationList = JsonConvert.DeserializeObject<List<Place>>(jsonResponse["businesses"].ToString());
            return locationList;

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
