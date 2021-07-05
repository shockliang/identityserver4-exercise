using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Bank.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://localhost:5001";
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
        }
    }
}