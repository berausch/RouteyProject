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
    public class autoPlace
    {
        public string Name { get; set; }


        public static List<string> GetAutocomplete(string userInput)
        {
            var client = new RestClient("https://api.yelp.com/v3/autocomplete?latitude=45.5206&longitude=-122.6774");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "9eec7f56-2754-d735-0518-55b8468755ef");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer Drh3Xrl-pkzYk-KyZtWgqtBx_uZHrbCzP7vFAnCdUydCSgdrmP1AV4_1sKKhKIuoKVqLNh9NKb0t2hPIybxt6CB9tqtShtUVguyOadwm4-t_0kI2mQSV5gtcR5O9WHYx");

            request.AddParameter("text", userInput);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var autoList1 = JsonConvert.DeserializeObject<List<autoPlace>>(jsonResponse["businesses"].ToString());


            List<string> autoList = autoList1.Select(s => s.Name).ToList();

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
