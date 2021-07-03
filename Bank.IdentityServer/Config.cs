using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using IdentityServer4.Models;

namespace Bank.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new("bankApi", "Customer Api for Bank"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("mySecret".Sha256())
                    },
                    AllowedScopes = {"bankApi"}
                }
            };
        }
    }
}