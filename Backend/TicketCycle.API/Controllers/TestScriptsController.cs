using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestScriptsController : ControllerBase
    {
        private readonly ITestScriptService _testScriptService;
        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public TestScriptsController(ITestScriptService testScriptService)
        {
            _testScriptService = testScriptService;
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(int ticketId) =>
            Ok(await _testScriptService.GetTestScriptsByTicketAsync(ticketId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var script = await _testScriptService.GetTestScriptByIdAsync(id);
            return script == null ? NotFound() : Ok(script);
        }

        [HttpPost]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> Create([FromBody] CreateTestScriptDto dto)
        {
            var script = await _testScriptService.CreateTestScriptAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = script.Id }, script);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestScriptDto dto) =>
            Ok(await _testScriptService.UpdateTestScriptAsync(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> Delete(int id)
        {
            await _testScriptService.DeleteTestScriptAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/steps")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> AddStep(int id, [FromBody] CreateTestLogDto dto) =>
            Ok(await _testScriptService.AddStepAsync(id, dto));

        [HttpPatch("{id}/steps/{stepId}")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> UpdateStep(int id, int stepId, [FromBody] UpdateTestLogDto dto) =>
            Ok(await _testScriptService.UpdateStepResultAsync(id, stepId, dto));

        [HttpDelete("{id}/steps/{stepId}")]
        [Authorize(Roles = "Tester")]
        public async Task<IActionResult> DeleteStep(int id, int stepId)
        {
            await _testScriptService.DeleteStepAsync(id, stepId);
            return NoContent();
        }
    }
}
