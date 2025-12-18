using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSLC.DTOs;
using StudentSLC.Services;

namespace StudentSLC.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO dto)
        {
            try
            {
                var room = await _roomService.CreateRoom(dto);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{roomName}")]
        public async Task<IActionResult> UpdateRoom(string roomName, [FromBody] UpdateRoomDTO dto)
        {
            try
            {
                var room = await _roomService.UpdateRoom(roomName, dto);
                return Ok(room);
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{roomName}")]
        public async Task<IActionResult> DeleteRoom(string roomName)
        {
            try
            {
                await _roomService.DeleteRoom(roomName);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }
    }
}
