using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentDto>> GetAttachmentsByTicketAsync(int ticketId);
        Task<AttachmentDto> UploadAttachmentAsync(int ticketId, IFormFile file, int uploadedById);
        Task DeleteAttachmentAsync(int id);
        Task<(byte[] fileBytes, string contentType, string fileName)> DownloadAttachmentAsync(int id);
    }
}
