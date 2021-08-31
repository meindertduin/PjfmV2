using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class DisconnectPlaybackGroupRequestHandler : IPlaybackRequestHandler<DisconnectPlaybackGroupRequest>
    {
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public DisconnectPlaybackGroupRequestHandler(IPlaybackGroupCollection playbackGroupCollection)
        {
            _playbackGroupCollection = playbackGroupCollection;
        }
        public Task HandleAsync(DisconnectPlaybackGroupRequest request)
        {
            _playbackGroupCollection.RemoveGebruikerFromGroup(new LuisteraarDto(request.GebruikerId,
                request.ConnectionId));

            // TODO: pause spotify player for user once it's implemented
            
            return Task.CompletedTask;
        }
    }

    public class DisconnectPlaybackGroupRequest : IPlaybackRequest
    {
        public string GebruikerId { get; set; } = null!;
        public Guid ConnectionId { get; set; }
    }
}