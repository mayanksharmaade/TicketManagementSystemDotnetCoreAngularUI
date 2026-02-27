using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _ticketService.GetAllTicketsAsync());

        [HttpGet("my")]
        public async Task<IActionResult> GetMyTickets() =>
            Ok(await _ticketService.GetMyTicketsAsync(GetCurrentUserId()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            var ticket = await _ticketService.CreateTicketAsync(dto, GetCurrentUserId());
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTicketStatusDto dto)
        {
            var ticket = await _ticketService.UpdateTicketStatusAsync(id, dto, GetCurrentUserId());
            return Ok(ticket);
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Assign(int id, [FromBody] AssignTicketDto dto)
        {
            var ticket = await _ticketService.AssignTicketAsync(id, dto, GetCurrentUserId());
            return Ok(ticket);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentDto dto)
        {
            await _ticketService.AddCommentAsync(id, dto, GetCurrentUserId());
            return Ok(new { message = "Comment added successfully." });
        }
    }
}
