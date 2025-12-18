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

        [Authorize(Roles = "student,teacher,keyholder,admin")]
        [HttpGet("groups/{groupName}")]
        public async Task<IActionResult> GetGroupSchedule(string groupName, [FromQuery] DateTime weekStart)
        {
            try
            {
                var events = await _service.GetGroupSchedule(groupName, weekStart);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "teacher, keyholder, admin")]
        [HttpGet("keyholders/{userCode}")]
        public async Task<IActionResult> GetKeyHolderSchedule(int userCode, [FromQuery] DateTime weekStart)
        {
            try
            {
                var events = await _service.GetKeyHolderSchedule(userCode, weekStart);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("rooms/{roomName}")]
        public async Task<IActionResult> GetRoomSchedule(string roomName, [FromQuery] DateTime weekStart)
        {
            try
            {
                var events = await _service.GetRoomSchedule(roomName, weekStart);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
