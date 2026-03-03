using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;

        public SprintsController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId) =>
            Ok(await _sprintService.GetSprintsByProjectAsync(projectId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sprint = await _sprintService.GetSprintByIdAsync(id);
            return sprint == null ? NotFound() : Ok(sprint);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateSprintDto dto)
        {
            var sprint = await _sprintService.CreateSprintAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = sprint.Id }, sprint);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSprintDto dto) =>
            Ok(await _sprintService.UpdateSprintAsync(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _sprintService.DeleteSprintAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/start")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Start(int id) =>
            Ok(await _sprintService.StartSprintAsync(id));

        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Complete(int id) =>
            Ok(await _sprintService.CompleteSprintAsync(id));

        [HttpPost("{id}/tickets/{ticketId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddTicket(int id, int ticketId)
        {
            await _sprintService.AddTicketToSprintAsync(id, ticketId);
            return Ok(new { message = "Ticket added to sprint." });
        }

        [HttpDelete("{id}/tickets/{ticketId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveTicket(int id, int ticketId)
        {
            await _sprintService.RemoveTicketFromSprintAsync(id, ticketId);
            return Ok(new { message = "Ticket removed from sprint." });
        }
    }
}
