using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models.DataTransferObjects;
using SpotifyPlayback.Requests.PlaybackRequestHandlers;

namespace Pjfm.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/playback")]
    public class PlaybackController : PjfmController
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;
        private readonly ISpotifyTokenService _spotifyTokenService;

        public PlaybackController(IPjfmControllerContext pjfmContext,
            IPlaybackRequestDispatcher playbackRequestDispatcher, ISpotifyTokenService spotifyTokenService) : base(
            pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _spotifyTokenService = spotifyTokenService;
        }

        [HttpGet("groups")]
        [ProducesResponseType(typeof(IEnumerable<PlaybackGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaybackGroups()
        {
            // TODO: later on we might have to add pagination, keep this in mind
            var playbackGroupsInfo =
                await _playbackRequestDispatcher.HandlePlaybackRequest(new GetPlaybackGroupsRequest());
            return Ok(playbackGroupsInfo.PlaybackGroups);
        }

        [HttpPut("{groupId:guid}/play")]
        public async Task<IActionResult> Play(string deviceId, Guid groupId)
        {
            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(PjfmPrincipal.Id);
            if (!accessTokenResult.IsSuccessful)
            {
                return Forbid();
            }
            
            var playResult = await _playbackRequestDispatcher.HandlePlaybackRequest(new PlayPlaybackForUserRequest()
            {
                DeviceId = deviceId,
                GroupId = groupId,
                Principal = PjfmPrincipal,
                SpotifyAccessToken = accessTokenResult.AccessToken,
            });

            if (!playResult.IsSuccessful)
            {
                // log result
            }

            return Ok();
        }
    }
}