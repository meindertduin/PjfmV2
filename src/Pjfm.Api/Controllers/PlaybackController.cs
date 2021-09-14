using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.Authentication;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;
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
        private readonly ISocketConnectionCollection _socketConnectionCollection;
        private readonly IPlaybackGroupCollection _playbackGroupCollection;

        public PlaybackController(IPjfmControllerContext pjfmContext,
            IPlaybackRequestDispatcher playbackRequestDispatcher,
            ISpotifyTokenService spotifyTokenService, ISocketConnectionCollection socketConnectionCollection,
            IPlaybackGroupCollection playbackGroupCollection) : base(
            pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _spotifyTokenService = spotifyTokenService;
            _socketConnectionCollection = socketConnectionCollection;
            _playbackGroupCollection = playbackGroupCollection;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Play(string deviceId, Guid groupId)
        {
            if (!_socketConnectionCollection.TryGetUserSocketConnection(PjfmPrincipal.Id, out var socketConnection))
            {
                return Conflict();
            }

            var accessTokenResult = await _spotifyTokenService.GetUserSpotifyAccessToken(PjfmPrincipal.Id);
            if (!accessTokenResult.IsSuccessful)
            {
                return Conflict();
            }

            var newListener = new ListenerDto(socketConnection!.ConnectionId, PjfmPrincipal, deviceId);
            var result = await _playbackRequestDispatcher.HandlePlaybackRequest(new AddListenerToGroupRequest()
            {
                GroupId = groupId, 
                NewListener = newListener,
                SpotifyAccessToken = accessTokenResult.AccessToken,
            });

            if (!result.IsSuccessful)
            {
                throw new Exception(result.Message);
            }

            _socketConnectionCollection.SetSocketConnectedGroupId(socketConnection.ConnectionId, groupId);

            return Ok();
        }

        [HttpPut("stop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Stop()
        {
            if (!_socketConnectionCollection.TryGetUserSocketConnection(PjfmPrincipal.Id, out var socketConnection))
            {
                return Conflict();
            }

            var connectionPlaybackGroupId = socketConnection.GetConnectedPlaybackGroupId();
            if (connectionPlaybackGroupId == null)
            {
                return Conflict();
            }
            
            await _playbackRequestDispatcher.HandlePlaybackRequest(new RemoveListenerFromGroupRequest()
            {
                UserGroupId = connectionPlaybackGroupId.Value,
                UserId = PjfmPrincipal.Id,
                ConnectionId = socketConnection.ConnectionId,
            });

            return Ok();
        }
    }
}