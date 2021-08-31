using System;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class LuisteraarDto
    {
        public string GebruikerId { get; set; }
        public Guid ConnectionId { get; set; }
        public LuisteraarDto(string gebruikerId, Guid connectionId)
        {
            GebruikerId = gebruikerId;
            ConnectionId = connectionId;
        }
    }
}