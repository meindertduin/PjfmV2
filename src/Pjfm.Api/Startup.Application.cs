using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.ApplicationUser;
using Pjfm.Application.Authentication;
using Pjfm.Application.GebruikerNummer;
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
            serviceCollection.AddTransient<ISpotifyTrackService, SpotifyTrackService>();
            serviceCollection.AddTransient<ISpotifyTokenService, SpotifyTokenService>();
            serviceCollection.AddTransient<IApplicationUserService, ApplicationUserService>();
            serviceCollection.AddTransient<ISpotifyAuthenticationRevoker, SpotifyAuthenticationRevoker>();
            serviceCollection.AddTransient<ISpotifyTrackClient, SpotifyTrackClient>();

            serviceCollection.AddSingleton<IUserTokenService, UserTokenService>();

            serviceCollection.AddScoped(x =>
                x.GetRequiredService<IHttpContextAccessor>().HttpContext!.User.GetPjfmPrincipal());
            
            serviceCollection.AddHttpClient();
        }
    }
}