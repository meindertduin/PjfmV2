using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Spotify;
using SpotifyPlayback.Interfaces;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify")]
    public class SpotifyPlayerController : PjfmController
    {
        private readonly ISpotifyPlaybackService _spotifyPlaybackService;

        public SpotifyPlayerController(IPjfmControllerContext pjfmContext, ISpotifyPlaybackService spotifyPlaybackService) : base(pjfmContext)
        {
            _spotifyPlaybackService = spotifyPlaybackService;
        }

        [HttpGet("devices")]
        [ProducesResponseType(typeof(GetDevicesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPlaybackDevices()
        {
            var devices = await _spotifyPlaybackService.GetUserDevices(PjfmPrincipal.Id);

            var deviceResponse = new GetDevicesResponse();
            var deviceModels = new List<DeviceModel>();
            
            foreach (var device in devices)
            {
                deviceModels.Add(new DeviceModel()
                {
                    DeviceId = device.Id,
                    DeviceName = device.Name,
                    IsActive = device.IsActive,
                });
            }

            deviceResponse.Devices = deviceModels;
            return Ok(deviceResponse);
        }
    }
}