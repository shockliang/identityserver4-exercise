using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bank.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://localhost:5001";

            await RequestPasswordTokenAsync(url);
            await RequestClientCredentialsTokenAsync(url);

            Console.Read();

        }

        private static async Task RequestPasswordTokenAsync(string url)
        {
            var httpClient = new HttpClient();
            var discoveryResourceOwner = await httpClient.GetDiscoveryDocumentAsync(url);

            if (discoveryResourceOwner.IsError)
            {
                Console.WriteLine(discoveryResourceOwner.Error);
                return;
            }
            
            // Grab a bearer token using ResourceOwnerPassword Grant Type
            var resourceOwnerTokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResourceOwner.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",
                UserName = "Jeremy",
                Password = "myPassword",
                Scope = "bankApi"
            });

            if (resourceOwnerTokenResponse.IsError)
            {
                Console.WriteLine(resourceOwnerTokenResponse.Error);
                return;
            }

            Console.WriteLine(resourceOwnerTokenResponse.Json);
        }

        private static async Task RequestClientCredentialsTokenAsync(string url)
        {
            var httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync(url);

            if (discovery.IsError)
            {
                Console.WriteLine(discovery.Error);
                return;
            }
            
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "mySecret",
                Scope = "bankApi"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(new {Id = 10, FirstName = "Shock", LastName = "Liang"}),
                Encoding.UTF8, 
                "application/json");

            var createCustomerResponse = await httpClient.PostAsync(
                "https://localhost:4001/api/customers", customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
                return;
            }

            var getCustomersResponse = await httpClient.GetAsync("https://localhost:4001/api/customers");
            if (!getCustomersResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomersResponse.StatusCode);
                return;
            }

            var content = await getCustomersResponse.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
        }
    }
}