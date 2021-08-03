using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Api.Controllers.Base;
using Pjfm.Appliaction.Spotify;
using Pjfm.Common.Authentication;
using Pjfm.Infrastructure.Services.Interfaces;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigureApplicationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISpotifyAuthenticationService, SpotifyAuthenticationService>();
            serviceCollection.AddTransient<IPjfmControllerContext, PjfmControllerContext>();

            serviceCollection.AddScoped(x =>
                x.GetRequiredService<IHttpContextAccessor>().HttpContext!.User.GetPjfmPrincipal());
        } 
    }
}