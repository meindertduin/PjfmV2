using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Exceptions;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class AddTracksToQueueRequestHandler : IPlaybackRequestHandler<AddTracksToQueueRequest, PlaybackRequestResult<AddTracksToQueueResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;
        private readonly IPlaybackScheduledTrackQueue _playbackScheduledTrackQueue;

        public AddTracksToQueueRequestHandler(IPlaybackGroupCollection playbackGroupCollection, IPlaybackScheduledTrackQueue playbackScheduledTrackQueue)
        {
            _playbackGroupCollection = playbackGroupCollection;
            _playbackScheduledTrackQueue = playbackScheduledTrackQueue;
        }
        
        public Task<PlaybackRequestResult<AddTracksToQueueResult>> HandleAsync(AddTracksToQueueRequest request)
        {
            var playbackGroup = _playbackGroupCollection.GetPlaybackGroup(request.GroupId);
            
            try
            {
                playbackGroup.AddRequestsToQueue(request.RequestedTracks, request.UserId);
            }
            catch (MaxRequestExceededException e)
            {
                return PlaybackRequestResult.FailAsync<AddTracksToQueueResult>("Max requests for user exceeded.");
            }
            
            return PlaybackRequestResult.SuccessAsync(new AddTracksToQueueResult(),
                "Successfully added tracks to queue");
        }
    }

    public class AddTracksToQueueRequest : IPlaybackRequest<PlaybackRequestResult<AddTracksToQueueResult>>
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; } = null!;
        public IEnumerable<SpotifyTrackDto> RequestedTracks { get; set; } = null!;
    }
    
    public class AddTracksToQueueResult
    {
    }
}