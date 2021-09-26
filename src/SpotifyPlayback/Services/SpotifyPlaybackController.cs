using System.Linq;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Services
{
    public class SpotifyPlaybackController : ISpotifyPlaybackController
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly ISocketDirector _socketDirector;
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;

        public SpotifyPlaybackController(IPlaybackGroupCollection playbackGroupCollection, ISocketDirector socketDirector, ISpotifyPlaybackService spotifyPlaybackService)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _socketDirector = socketDirector;
            _spotifyPlaybackService = spotifyPlaybackService;
        }
        
        public Task PlaySpotifyTrackForUsers(PlaybackScheduledTrack playbackScheduledTrack)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(playbackScheduledTrack.GroupId);
            var listeners = playbackGroup.GetGroupListeners();
            var connectedIds = playbackGroup.GetJoinedConnectionIds();

            var playbackUpdateMessage = CreatePlaybackUpdateMessage(playbackGroup);

            _socketDirector.BroadCastMessageOverConnections(playbackUpdateMessage, connectedIds);

            return _spotifyPlaybackService.PlayNextTrackForUsers(listeners.ToArray(), playbackScheduledTrack.SpotifyTrack.SpotifyTrackId);
        }
        private SocketMessage<PlaybackUpdateMessageBody> CreatePlaybackUpdateMessage(IPlaybackGroup playbackGroup)
        {
            var playbackGroupInfo = playbackGroup.GetPlaybackGroupInfo();
            
            var playbackUpdateMessage = new SocketMessage<PlaybackUpdateMessageBody>()
            {
                MessageType = MessageType.PlaybackInfo,
                Body = new PlaybackUpdateMessageBody()
                {
                    CurrentlyPlayingTrack = playbackGroupInfo.CurrentlyPlayingTrack,
                    GroupId = playbackGroupInfo.GroupId,
                    GroupName = playbackGroupInfo.GroupName,
                    QueuedTracks = playbackGroupInfo.QueuedTracks,
                }
            };
            return playbackUpdateMessage;
        }
    }
}