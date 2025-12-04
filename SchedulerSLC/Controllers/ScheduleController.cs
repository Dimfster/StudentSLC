using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSLC.DTOs;
using StudentSLC.Services;

namespace StudentSLC.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _service;

        public ScheduleController(ScheduleService service)
        {
            _service = service;
        }

        // GET: api/schedule/groups/{groupName}
        [Authorize(Roles = "student,teacher,keyholder,admin")]
        [HttpGet("groups/{groupName}")]
        public async Task<IActionResult> GetGroupSchedule(string groupName)
        {
            try
            {
                var events = await _service.GetGroupSchedule(groupName);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/schedule/keyholders/{userCode}
        [Authorize(Roles = "keyholder, admin")]
        [HttpGet("keyholders/{userCode}")]
        public async Task<IActionResult> GetKeyHolderSchedule(int userCode)
        {
            try
            {
                var events = await _service.GetKeyHolderSchedule(userCode);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/schedule/rooms/{roomName}
        [Authorize]
        [HttpGet("rooms/{roomName}")]
        public async Task<IActionResult> GetRoomSchedule(string roomName)
        {
            try
            {
                var events = await _service.GetRoomSchedule(roomName);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
