using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pjfm.Application.GebruikerNummer.Models;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback.Requests.PlaybackRequestHandlers
{
    public class AddTracksToQueueRequestHandler : IPlaybackRequestHandler<AddTracksToQueueRequest, PlaybackRequestResult<AddTracksToQueueResult>>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public AddTracksToQueueRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        
        public Task<PlaybackRequestResult<AddTracksToQueueResult>> HandleAsync(AddTracksToQueueRequest request)
        {
            var addedTracks = _playbackGroupCollection.AddTracksToQueue(request.RequestedTracks, request.GroupId);

            if (!addedTracks)
            {
                return PlaybackRequestResult.FailAsync<AddTracksToQueueResult>("Failed to add tracks to queue.");
            }

            return PlaybackRequestResult.SuccessAsync(new AddTracksToQueueResult(),
                "Successfully added tracks to queue");
        }
    }

    public class AddTracksToQueueRequest : IPlaybackRequest<PlaybackRequestResult<AddTracksToQueueResult>>
    {
        public Guid GroupId { get; set; }
        public IEnumerable<SpotifyTrackDto> RequestedTracks { get; set; } = null!;
    }
    
    public class AddTracksToQueueResult
    {
    }
}