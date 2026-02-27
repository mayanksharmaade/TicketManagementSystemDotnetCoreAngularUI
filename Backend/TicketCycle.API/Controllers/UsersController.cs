using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _userService.GetAllUsersAsync());

        [HttpGet("developers")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetDevelopers() =>
            Ok(await _userService.GetDevelopersAsync());

        [HttpGet("testers")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetTesters() =>
            Ok(await _userService.GetTestersAsync());
    }
}
