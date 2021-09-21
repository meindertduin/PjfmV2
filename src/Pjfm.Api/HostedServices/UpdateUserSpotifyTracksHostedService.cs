using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pjfm.Application.ApplicationUser;
using Pjfm.Application.GebruikerNummer;

namespace Pjfm.Api.HostedServices
{
    public class UpdateUserSpotifyTracksHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer? _timer;

        public UpdateUserSpotifyTracksHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void Execute(object? state)
        {
            // Don't care about awaiting this, because this method will only be called once a day (as of writing this comment).
            // And since this function cannot be asynchronous we fire the local async function off without awaiting it.
#pragma warning disable 4014
            ExecuteAsync();
#pragma warning restore 4014
            
            async Task ExecuteAsync()
            {
                using var scope = _serviceProvider.CreateScope();

                var applicationUserService = scope.ServiceProvider.GetRequiredService<IApplicationUserService>();
                var spotifyTrackService = scope.ServiceProvider.GetRequiredService<ISpotifyTrackService>();
                
                // TODO: if the application surpasses the 1000-2000 or so users, not all the users should be retrieved at once but rather be paginated.
                var users = await applicationUserService.GetApplicationUsers(new GetUsersRequest()
                {
                    SinceLastLoginDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                    SpotifyAuthenticated = true,
                });
                foreach (var user in users)
                {
                    await spotifyTrackService.UpdateUserSpotifyTracks(user.Id);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}