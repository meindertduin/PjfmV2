using System;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class ListenerDto
    {
        public string UserId { get; set; }
        public Guid ConnectionId { get; set; }
        public ListenerDto(string userId, Guid connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }
    }
}