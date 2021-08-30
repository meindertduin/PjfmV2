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
            return Task.FromResult(_playbackGroupCollection.RemoveGebruikerFromGroup(new LuisteraarDto(request.GebruikerId, request.ConnectionId)));
        }
    }

    public class DisconnectPlaybackGroupRequest : IPlaybackRequest
    {
        public string GebruikerId { get; set; }
        public Guid ConnectionId { get; set; }
    }
}