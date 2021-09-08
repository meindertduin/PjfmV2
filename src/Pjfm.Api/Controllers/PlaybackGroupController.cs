using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using SpotifyPlayback.Interfaces;

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
    }
}