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
            services.AddTransient<IPlaybackScheduledTaskQueue, PlaybackScheduledTaskQueue>();
            services.AddTransient<IPlaybackQueue, PlaybackQueue>();
            
            services.AddSingleton<ISocketDirector, PlaybackSocketDirector>();
            services.AddSingleton<IPlaybackGroepCollection, PlaybackGroepCollection>();
            
            services.AddHostedService<SpotifyPlaybackHostedService>();

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
            });
        }
    }
}