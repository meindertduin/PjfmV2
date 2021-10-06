using Newtonsoft.Json;

namespace Pjfm.Application.Common
{
    public static class SpotifyApiHelpers
    {
        public static JsonSerializerSettings GetSpotifySerializerSettings()
        {
            return new()
            {
                // TODO: hopefully this doesnt break anything X__X, but certainly needs to get fixed
                // ContractResolver = new CustomContractResolver()
                // {
                //     NamingStrategy = new SnakeCaseNamingStrategy(),
                // },
                // Formatting = Formatting.Indented,
            };
        }
    }
}