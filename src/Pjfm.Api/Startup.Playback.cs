using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback;
using SpotifyPlayback.Interfaces;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigurePlayback(IServiceCollection services)
        {
            services.AddSingleton<ISocketDirector, PlaybackSocketDirector>();
        }
    }
}