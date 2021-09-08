using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackController : ISpotifyPlaybackController
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISocketDirector _socketDirector;

        public SpotifyPlaybackController(IPlaybackGroupCollection playbackGroupCollection, IServiceProvider serviceProvider, ISocketDirector socketDirector)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _serviceProvider = serviceProvider;
            _socketDirector = socketDirector;
        }
        
        public Task PlaySpotifyTrackForUsers(PlaybackScheduledTrack playbackScheduledTrack)
        {
            using var scope = _serviceProvider.CreateScope();
            var spotifyPlaybackService = scope.ServiceProvider.GetRequiredService<ISpotifyPlaybackService>();
            var listeners = _playbackGroupCollection.GetGroupListeners(playbackScheduledTrack.GroupId);
            var playbackGroupInfo = _playbackGroupCollection.GetPlaybackGroupInfo(playbackScheduledTrack.GroupId);
            var connectedIds = _playbackGroupCollection.GetGroupJoinedConnectionIds(playbackScheduledTrack.GroupId);

            var playbackUpdateMessage = new SocketMessage<PlaybackUpdateMessageBody>()
            {
                MessageType = MessageType.PlaybackInfo,
                Body = new PlaybackUpdateMessageBody()
                {
                    CurrentlyPlayingTrack = playbackGroupInfo.CurrentlyPlayingTrack,
                    GroupId = playbackGroupInfo.GroupId,
                    GroupName = playbackGroupInfo.GroupName,
                }
            };
            
            _socketDirector.BroadCastMessageOverConnections(playbackUpdateMessage, connectedIds);

            return spotifyPlaybackService.PlayNextTrackForUsers(listeners.ToArray(), playbackScheduledTrack.SpotifyTrack.SpotifyTrackId);
        }

        public Task PauseSpotifyPlayerUser()
        {
            throw new System.NotImplementedException();
        }
    }
}