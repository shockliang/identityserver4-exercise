using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Bank.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        
        public static List<TestUser> GetUsers()
        {
            return new()
            {
                new()
                {
                    SubjectId = "1",
                    Username = "Jeremy",
                    Password = "myPassword"
                },
                new()
                {
                    SubjectId = "2",
                    Username = "Molly",
                    Password = "MollyIsMe"
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new(name: "bankApi", displayName: "Customer Api for Bank"),
            };
        }

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
                new()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("mySecret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "bankApi"
                    }
                },
                new()
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"bankApi"}
                },
                new Client()
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"http://localhost:7000/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:7000/signout-callback-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
            };
        }
    }
}