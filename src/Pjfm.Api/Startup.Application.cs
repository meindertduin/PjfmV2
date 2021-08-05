using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.Authentication;
using Pjfm.Application.Spotify;
using Pjfm.Common.Authentication;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigureApplicationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISpotifyAuthenticationService, SpotifyAuthService>();
            serviceCollection.AddTransient<IPjfmControllerContext, PjfmControllerContext>();
            serviceCollection.AddTransient<ISpotifyApiClient, SpotifyApiClient>();

            serviceCollection.AddSingleton<IGebruikerTokenService, GebruikerTokenService>();

            serviceCollection.AddScoped(x =>
                x.GetRequiredService<IHttpContextAccessor>().HttpContext!.User.GetPjfmPrincipal());
            
            serviceCollection.AddHttpClient();
        } 
    }
}