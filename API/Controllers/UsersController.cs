using Application.Servces.UserService;
using Application.Servces.UserService.DTOs;
using Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "TechnicianAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto input)
        {
            await _userService.CreateUser(input);
            return Ok();
        }


        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto input)
        {
            await _userService.UpdateUser(input);
            return Ok();
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }


        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(string? name, SystemRole? role)
        {
            var users = await _userService.GetAllUsers(name, role);
            return Ok(users);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordInputDto input)
        {
            await _userService.ChangePasswordAsync(input);
            return Ok();
        }
    }
}
