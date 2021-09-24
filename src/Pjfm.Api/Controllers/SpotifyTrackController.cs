using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Authentication;
using Pjfm.Api.Controllers.Base;
using Pjfm.Api.Models.SpotifyTrack;
using Pjfm.Application.GebruikerNummer;

namespace Pjfm.Api.Controllers
{
    [Authorize(WellKnownPolicies.SpotifyAuthenticatedUser)]
    [Microsoft.AspNetCore.Components.Route("api/spotify/tracks")]
    public class SpotifyTrackController : PjfmController
    {
        private readonly ISpotifyTrackClient _spotifyTrackClient;

        public SpotifyTrackController(IPjfmControllerContext pjfmContext, ISpotifyTrackClient spotifyTrackClient) : base(pjfmContext)
        {
            _spotifyTrackClient = spotifyTrackClient;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(SearchTracksResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSpotifyTrack(string query)
        {
            var result = await _spotifyTrackClient.SearchSpotifyTracks(query, PjfmPrincipal.Id);
            var searchResult = new SearchTracksResult()
            {
                Tracks = result,
            };
            
            return Ok(searchResult);
        }
    }
}