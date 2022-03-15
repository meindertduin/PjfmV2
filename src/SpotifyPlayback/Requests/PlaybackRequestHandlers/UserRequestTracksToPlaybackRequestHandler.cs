using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Exceptions;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
using SpotifyPlayback.Models.Socket;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class UserRequestTracksToPlaybackRequestHandler : IPlaybackRequestHandler<UserRequestTracksToPlaybackRequest, PlaybackRequestResult<UserRequestTracksToPlaybackRequestResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue;
        private readonly ISocketDirector _socketDirector;

        public UserRequestTracksToPlaybackRequestHandler(IPlaybackGroupCollection playbackGroupCollection, IPlaybackScheduledTrackQueue playbackScheduledTrackQueue, ISocketDirector socketDirector)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _playbackScheduledTrackQueue = playbackScheduledTrackQueue;
            _socketDirector = socketDirector;
        }
        
        public Task<PlaybackRequestResult<UserRequestTracksToPlaybackRequestResult>> HandleAsync(UserRequestTracksToPlaybackRequest request)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            
            try
            {
                playbackGroup.AddRequestsToQueue(request.RequestedTracks, request.UserId);
            }
            catch (MaxRequestExceededException e)
            {
                return PlaybackRequestResult.FailAsync<UserRequestTracksToPlaybackRequestResult>("Max requests for user exceeded.");
            }

            var groupJoinedConnections = playbackGroup.GetJoinedConnectionIds();

            var playbackGroupInfo = playbackGroup.GetPlaybackGroupInfo();
            var updateMessage = new SocketMessage<PlaybackUpdateMessageBody>()
            {
                MessageType = MessageType.PlaybackInfo,
                Body = new PlaybackUpdateMessageBody()
                {
                    GroupId = playbackGroupInfo.GroupId,
                    GroupName = playbackGroupInfo.GroupName,
                    CurrentlyPlayingTrack = playbackGroupInfo.CurrentlyPlayingTrack,
                    QueuedTracks = playbackGroupInfo.QueuedTracks,
                }
            };
            
            _socketDirector.BroadCastMessageOverConnections(updateMessage, groupJoinedConnections);
            
            return PlaybackRequestResult.SuccessAsync(new UserRequestTracksToPlaybackRequestResult(),
                "Successfully added tracks to queue");
        }
    }

    public class UserRequestTracksToPlaybackRequest : IPlaybackRequest<PlaybackRequestResult<UserRequestTracksToPlaybackRequestResult>>
    {
        public string GroupId { get; set; }
        public string UserId { get; set; } = null!;
        public IEnumerable<SpotifyTrackDto> RequestedTracks { get; set; } = null!;
    }
    
    public class UserRequestTracksToPlaybackRequestResult
    {
    }
}