using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSLC.DTOs;
using StudentSLC.Services;

namespace StudentSLC.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            try
            {
                var user = await _userService.CreateUser(dto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{userCode}")]
        public async Task<IActionResult> UpdateUser(int userCode, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                var user = await _userService.UpdateUser(userCode, dto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{userCode}")]
        public async Task<IActionResult> DeleteUser(int userCode)
        {
            try
            {
                await _userService.DeleteUser(userCode);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var users = await _userService.GetAllTeachers();
            return Ok(users);
        }
    }
}
