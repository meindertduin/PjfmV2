using Microsoft.Extensions.DependencyInjection;
using Pjfm.Api.Extensions;
using SpotifyPlayback;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Requests;
using SpotifyPlayback.Services;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigurePlayback(IServiceCollection services)
        {
            services.AddTransient<IPlaybackRequestDispatcher, PlaybackRequestDispatcher>();
            services.AddTransient<IPlaybackQueue, PlaybackQueue>();
            services.AddTransient<ISpotifyPlaybackClient, SpotifyPlaybackClient>();
            services.AddTransient<ISpotifyPlaybackService, SpotifyPlaybackService>();
            services.AddTransient<ISpotifyPlaybackController, SpotifyPlaybackController>();
            services.AddTransient<ISocketRequestHandler, SocketRequestHandler>();
            
            services.AddSingleton<IPlaybackScheduledTrackQueue, PlaybackScheduledTrackQueue>();
            services.AddSingleton<ISocketDirector, PlaybackSocketDirector>();
            services.AddSingleton<IPlaybackGroupCollection, PlaybackGroupCollection>();
            services.AddSingleton<ISocketConnectionCollection, SocketConnectionCollection>();
            
            services.AddHostedService<SpotifyPlaybackHostedService>();
            services.AddHostedService<ConnectionWatcherHostedService>();

            services.Scan(a =>
            {
                a.FromAssemblies(typeof(SpotifyPlaybackHostedService).Assembly)
                    .AddClasses(p => p.AssignableTo(typeof(IPlaybackRequestHandler<,>)))
                    .AsClosedTypeOf(typeof(IPlaybackRequestHandler<,>))
                    .WithTransientLifetime();
                
                a.FromAssemblies(typeof(SpotifyPlaybackHostedService).Assembly)
                    .AddClasses(p => p.AssignableTo(typeof(IPlaybackRequestHandler<>)))
                    .AsClosedTypeOf(typeof(IPlaybackRequestHandler<>))
                    .WithTransientLifetime();

                a.FromAssemblies(typeof(SpotifyPlaybackHostedService).Assembly)
                    .AddClasses(p => p.AssignableTo(typeof(IPlaybackSocketRequestHandler<>)))
                    .AsClosedTypeOf(typeof(IPlaybackSocketRequestHandler<>))
                    .WithTransientLifetime();
            });
        }
    }
}