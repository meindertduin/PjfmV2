using IdentityServer4.Stores.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pjfm.Application.Common
{
    public static class SpotifyApiHelpers
    {
        public static JsonSerializerSettings GetSpotifySerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                ContractResolver = new CustomContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }
    }
}