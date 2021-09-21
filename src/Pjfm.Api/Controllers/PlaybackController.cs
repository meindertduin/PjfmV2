using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
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
        private readonly ISocketConnectionCollection _socketConnectionCollection;

        public PlaybackController(IPjfmControllerContext pjfmContext, IPlaybackRequestDispatcher playbackRequestDispatcher,
            ISocketConnectionCollection socketConnectionCollection) : base(
            pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _socketConnectionCollection = socketConnectionCollection;
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

            var newListener = new ListenerDto(socketConnection!.ConnectionId, PjfmPrincipal, deviceId);
            var result = await _playbackRequestDispatcher.HandlePlaybackRequest(new AddListenerToGroupRequest()
            {
                GroupId = groupId, 
                NewListener = newListener,
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