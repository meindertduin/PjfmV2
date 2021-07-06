using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Pjfm.Api.Authentication
{
    public static class ApiIdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), 
                new IdentityResources.Profile(), 
            };
        }
        
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            
            return new List<ApiScope>
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, new[]
                {
                    JwtClaimTypes.PreferredUserName,
                })
            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "pjfm_web_client",
                    ClientSecrets = new List<Secret> {new ("test_secret")},
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = new[]
                    {
                        "https://localhost:5005/signin-oidc",
                    },
                    PostLogoutRedirectUris = new[]
                    {
                        "https://localhost:5005/signout-callback-oidc",
                    },

                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.LocalApi.ScopeName,
                    },
                    AllowedCorsOrigins = new List<string>()
                    {
                        "https://localhost:5005",
                    },

                    AlwaysIncludeUserClaimsInIdToken = true,

                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                }
            };
        }
    }
}