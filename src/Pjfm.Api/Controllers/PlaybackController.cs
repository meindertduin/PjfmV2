using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Requests.SocketRequestHandlers;

namespace Pjfm.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/playback")]
    public class PlaybackController : PjfmController
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;

        public PlaybackController(IPjfmControllerContext pjfmContext, IPlaybackRequestDispatcher playbackRequestDispatcher) : base(pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
        }

        [HttpGet("groups")]
        [ProducesResponseType(typeof(IEnumerable<PlaybackGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaybackGroups()
        {
            // TODO: later on we might have to add pagination, keep this in mind
            var playbackGroupsInfo = await _playbackRequestDispatcher.HandlePlaybackRequest(new GetPlaybackGroupsRequest());
            return Ok(playbackGroupsInfo.PlaybackGroups);
        }
    }
}