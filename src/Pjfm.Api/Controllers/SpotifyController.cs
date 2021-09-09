using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Spotify;
using Pjfm.Application.Spotify;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/spotify")]
    public class SpotifyController : PjfmController
    {
        private readonly ISpotifyService _spotifyService;

        public SpotifyController(IPjfmControllerContext pjfmContext, ISpotifyService spotifyService) : base(pjfmContext)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet("tracks/update")]
        public IActionResult UpdateUserSpotifyTracks()
        {
            return Ok();
        }

        [HttpGet("devices")]
        [ProducesResponseType(typeof(GetDevicesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPlaybackDevices()
        {
            var devices = await _spotifyService.GetUserDevices(PjfmPrincipal.Id);

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