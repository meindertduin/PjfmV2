using System.Collections.Generic;

namespace Pjfm.Api.Models.Spotify
{
    public class GetDevicesResponse
    {
        public IEnumerable<DeviceModel> Devices { get; set; } = null!;
    }
}