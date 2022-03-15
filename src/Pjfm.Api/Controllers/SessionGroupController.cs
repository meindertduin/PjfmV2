using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Domain.SessionGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;
using Pjfm.Application.ApplicationUser;

namespace Pjfm.Api.Controllers
{
    [Authorize]
    [Route("api/session")]
    public class SessionGroupController : PjfmController
    {
        private readonly ISessionGroupRepository _sessionGroupRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SessionGroupController(IPjfmControllerContext pjfmContext, ISessionGroupRepository sessionGroupRepository, 
            UserManager<ApplicationUser> userManager) : base(pjfmContext)
        {
            _sessionGroupRepository = sessionGroupRepository;
            _userManager = userManager;
        }

        [HttpGet("GetParticipants/{groupId}")]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetParticipants(string groupId)
        {
            var group = await _sessionGroupRepository.FindSessionGroupById(groupId);

            if (group == null)
            {
                return NotFound();
            }

            var participants = group.FillerQueueParticipants.Select(p => new ApplicationUserDto()
            {
                UserName = p.UserName,
                UserId = p.Id,
            });

            return Ok(participants);
        }

        [HttpPut("AddFillerQueueParticipant/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddFillerQueueParticipant(string userId, string groupId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            var group = await _sessionGroupRepository.FindSessionGroupById(groupId);

            if (group == null)
            {
                return NotFound();
            }
            
            // TODO check if user is allowed to add other user to session

            await _sessionGroupRepository.AddFillerQueueParticipant(groupId, user);
            
            return Ok();
        }
        
        [HttpPut("RemoveFillerQueueParticipant/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFillerQueueParticipant(string userId, string groupId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            var group = await _sessionGroupRepository.FindSessionGroupById(groupId);

            if (group == null)
            {
                return NotFound();
            }
            
            // TODO check if user is allowed to add other user to session

            await _sessionGroupRepository.RemoveFillerQueueParticipant(groupId, user);
            
            return Ok();
        }
    }
}