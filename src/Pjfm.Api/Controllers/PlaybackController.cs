using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.Playback;
using Pjfm.Application.ApplicationUser;
using Pjfm.Application.GebruikerNummer;
using Pjfm.Application.GebruikerNummer.Models;
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
        private readonly ISpotifyTrackClient _spotifyTrackClient;

        public PlaybackController(IPjfmControllerContext pjfmContext, IPlaybackRequestDispatcher playbackRequestDispatcher,
            ISocketConnectionCollection socketConnectionCollection, ISpotifyTrackClient spotifyTrackClient) : base(
            pjfmContext)
        {
            _playbackRequestDispatcher = playbackRequestDispatcher;
            _socketConnectionCollection = socketConnectionCollection;
            _spotifyTrackClient = spotifyTrackClient;
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

            socketConnection.SetListeningPlaybackGroupId(groupId);

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

            var connectionPlaybackGroupId = socketConnection.GetListeningPlaybackGroupId();
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

        [HttpPut("track-request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PlaybackTrackRequest(PlaybackTrackRequest trackRequest)
        {
            if (!_socketConnectionCollection.TryGetUserSocketConnection(PjfmPrincipal.Id, out var socketConnection))
            {
                return Conflict();
            }

            var joinedPlaybackGroupId = socketConnection.GetJoinedPlaybackGroupId();
            if (joinedPlaybackGroupId == null)
            {
                return Conflict();
            }
            
            var tracks = (await _spotifyTrackClient.GetTracks(trackRequest.TrackIds, PjfmPrincipal.Id)).ToList();

            if (!tracks.Any())
            {
                return BadRequest();
            }
            
            foreach (var requestedTrack in tracks)
            {
                requestedTrack.TrackType = TrackType.Request;
                requestedTrack.User = new ApplicationUserDto()
                {
                    UserId = PjfmPrincipal.Id, 
                    UserName = PjfmPrincipal.UserName
                };
            }

            var result = await _playbackRequestDispatcher.HandlePlaybackRequest(new UserRequestTracksToPlaybackRequest()
            {
                GroupId = joinedPlaybackGroupId.Value,
                RequestedTracks = tracks,
                UserId = PjfmPrincipal.Id,
            });

            if (!result.IsSuccessful)
            {
                return Conflict();
            }

            return Ok();
        }
    }
}