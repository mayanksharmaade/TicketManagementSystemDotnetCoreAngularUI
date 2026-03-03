using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtasksController : ControllerBase
    {
        private readonly ISubtaskService _subtaskService;

        public SubtasksController(ISubtaskService subtaskService)
        {
            _subtaskService = subtaskService;
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(int ticketId) =>
            Ok(await _subtaskService.GetSubtasksByTicketAsync(ticketId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subtask = await _subtaskService.GetSubtaskByIdAsync(id);
            return subtask == null ? NotFound() : Ok(subtask);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubtaskDto dto)
        {
            var subtask = await _subtaskService.CreateSubtaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = subtask.Id }, subtask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubtaskDto dto) =>
            Ok(await _subtaskService.UpdateSubtaskAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subtaskService.DeleteSubtaskAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id) =>
            Ok(await _subtaskService.ToggleCompletionAsync(id));

        [HttpGet("ticket/{ticketId}/completion")]
        public async Task<IActionResult> GetCompletion(int ticketId) =>
            Ok(new { percentage = await _subtaskService.GetCompletionPercentageAsync(ticketId) });
    }

}
