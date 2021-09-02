using System;
using System.Threading.Tasks;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;

namespace SpotifyPlayback.Requests.Handlers
{
    public class DisconnectPlaybackGroupRequestHandler : IPlaybackRequestHandler<DisconnectPlaybackGroupRequest>
    {
        private readonly IPlaybackGroepCollection _playbackGroepCollection;

        public DisconnectPlaybackGroupRequestHandler(IPlaybackGroepCollection playbackGroepCollection)
        {
            _playbackGroepCollection = playbackGroepCollection;
        }
        public Task HandleAsync(DisconnectPlaybackGroupRequest request)
        {
            _playbackGroepCollection.RemoveGebruikerFromGroup(new LuisteraarDto(request.GebruikerId,
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