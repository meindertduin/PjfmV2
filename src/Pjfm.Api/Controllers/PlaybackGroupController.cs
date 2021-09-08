using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Requests.PlaybackRequestHandlers;

namespace Pjfm.Api.Controllers
{
    [Route("api/playback/group")]
    [Authorize]
    public class PlaybackGroupController : PjfmController
    {
        private readonly IPlaybackRequestDispatcher _playbackRequestDispatcher;
        private readonly ISocketDirector _socketDirector;

        public PlaybackGroupController(IPjfmControllerContext pjfmContext, IPlaybackRequestDispatcher playbackRequestDispatcher, ISocketDirector socketDirector) : base(pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _socketDirector = socketDirector;
        }

        [HttpPut("{groupId:guid}/join")]
        public async Task<IActionResult> Join(Guid groupId)
        {
            var sessionId = HttpContext.Session.Id;
            var hasSocketConnection = _socketDirector.TryGetUserSocketConnection(PjfmContext.PjfmPrincipal.Id, out var socketConnection);
            if (!hasSocketConnection)
            {
                return NotFound();
            }
            
            var result = await _playbackRequestDispatcher.HandlePlaybackRequest(new JoinPlaybackGroupRequest()
            {
                GroupId = groupId,
                UserId = PjfmContext.PjfmPrincipal.Id,
                ConnectionId = socketConnection!.ConnectionId,
            });

            if (result.SuccessfullyJoined)
            {
                return Ok();
            }
            
            return BadRequest();
        }
    }
}