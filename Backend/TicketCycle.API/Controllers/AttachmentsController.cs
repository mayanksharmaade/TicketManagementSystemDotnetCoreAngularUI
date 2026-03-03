using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketCycle.Application.Interfaces;

namespace TicketCycle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(int ticketId) =>
            Ok(await _attachmentService.GetAttachmentsByTicketAsync(ticketId));

        [HttpPost("ticket/{ticketId}")]
        public async Task<IActionResult> Upload(int ticketId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            var attachment = await _attachmentService.UploadAttachmentAsync(ticketId, file, CurrentUserId);
            return Ok(attachment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var (fileBytes, contentType, fileName) = await _attachmentService.DownloadAttachmentAsync(id);
            return File(fileBytes, contentType, fileName);
        }
    }

}
