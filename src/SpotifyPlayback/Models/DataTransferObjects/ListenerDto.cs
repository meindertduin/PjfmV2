using System;
using Pjfm.Common.Authentication;

namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class ListenerDto
    {
        public IPjfmPrincipal Principal { get; set; }
        public Guid ConnectionId { get; set; }
        public string DeviceId { get; set; }

        public ListenerDto(Guid connectionId, IPjfmPrincipal principal, string deviceId)
        {
            ConnectionId = connectionId;
            Principal = principal;
            DeviceId = deviceId;
        }
    }
}