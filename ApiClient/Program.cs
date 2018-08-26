using Bogus;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using System;
using System.Configuration;

namespace ApiClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string apiUrlKey = "apiUrl";
                string input;
                var random = new Randomizer();
                var randomDate = new Bogus.DataSets.Date();

                var apiUrl = ConfigurationManager.AppSettings[apiUrlKey];
                var restClient = new RestClient(apiUrl);

                Console.WriteLine("Press ENTER without specifying value to exit...\n\n");
                Console.WriteLine("How many request models you want to send? Type in number: ");

                while (!string.IsNullOrEmpty(input = Console.ReadLine())) {
                    var result = 0;
                    var parsed = int.TryParse(input, out result);

                    if (!parsed)
                    {
                        Console.WriteLine("Value you've entered is not valid INT");
                    }
                    else
                    {                        
                        for (var i = 0; i < result; i++)
                        {
                            Console.WriteLine($"Sending request no. {i}...");

                            var request = new RestSharp.RestRequest("data", Method.POST);
                            request.JsonSerializer = new NewtonsoftJsonSerializer();

                            request.AddJsonBody(new RequestModel[] {new RequestModel
                            {
                                Index = random.Int(0, int.MaxValue),
                                Date = randomDate.Future(),
                                Name = random.String2(20),
                                Visits = random.Int(0, int.MaxValue),
                            } });

                            var response = restClient.Execute(request);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Console.WriteLine($"Request no. {i} sent succesfully.");
                            }
                            else
                            {
                                throw new Exception($"Got status code <{response.StatusCode}> instead of <{System.Net.HttpStatusCode.Created}>.");
                            }
                        }

                        Console.WriteLine("Finished sending requests to API!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
