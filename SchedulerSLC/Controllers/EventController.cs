using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSLC.DTOs;
using StudentSLC.Services;

namespace StudentSLC.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        // POST: api/events
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
        {
            try
            {
                var ev = await _eventService.CreateEvent(dto);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PATCH: api/events/{eventName}
        [Authorize(Roles = "admin")]
        [HttpPatch("{eventName}")]
        public async Task<IActionResult> UpdateEvent(
            string eventName,
            [FromBody] UpdateEventDTO dto)
        {
            try
            {
                var ev = await _eventService.UpdateEvent(eventName, dto);
                return Ok(ev);
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

        /// DELETE: api/events/{eventName}
        [Authorize(Roles = "admin")]
        [HttpDelete("{eventName}")]
        public async Task<IActionResult> DeleteEvent(string eventName)
        {
            try
            {
                await _eventService.DeleteEvent(eventName);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
