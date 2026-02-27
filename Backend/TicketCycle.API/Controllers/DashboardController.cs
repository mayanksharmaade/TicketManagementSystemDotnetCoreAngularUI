using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public DashboardController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet("manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetManagerDashboard() =>
            Ok(await _ticketService.GetManagerDashboardAsync());

        [HttpGet("developer")]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> GetDeveloperDashboard() =>
            Ok(await _ticketService.GetDeveloperDashboardAsync(GetCurrentUserId()));

        [HttpGet("tester")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> GetTesterDashboard() =>
            Ok(await _ticketService.GetTesterDashboardAsync(GetCurrentUserId()));
    }
}
