using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSLC.DTOs;
using StudentSLC.Models;
using StudentSLC.Services;

namespace StudentSLC.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupsController(GroupService groupService)
        {
            _groupService = groupService;
        }

        // POST: api/groups
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO dto)
        {
            try
            {
                var group = await _groupService.CreateGroup(dto);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PATCH: api/groups/{groupName}
        [Authorize(Roles = "admin")]
        [HttpPatch("{groupName}")]
        public async Task<IActionResult> UpdateGroup(string groupName, [FromBody] UpdateGroupDTO dto)
        {
            try
            {
                var group = await _groupService.UpdateGroup(groupName, dto);
                return Ok(group);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/groups/{groupName}
        [Authorize(Roles = "admin")]
        [HttpDelete("{groupName}")]
        public async Task<IActionResult> DeleteGroup(string groupName)
        {
            try
            {
                await _groupService.DeleteGroup(groupName);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
