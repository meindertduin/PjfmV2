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

        [HttpPut("SetFillerQueueParticipants/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetFillerQueueParticipants(string[] userIds, string groupId)
        {
            var group = await _sessionGroupRepository.FindSessionGroupById(groupId);

            if (group == null)
            {
                return NotFound();
            }
            
            // TODO check if user is allowed to add other user to session
            
            List<Task<ApplicationUser>> findUserTasks = new ();

            foreach (var userId in userIds)
            {
                findUserTasks.Add(_userManager.FindByIdAsync(userId));
            }

            var users = await Task.WhenAll(findUserTasks);

            await _sessionGroupRepository.SetFillerQueueParticipants(groupId, users.ToList());
            
            return Ok();
        }
    }
}