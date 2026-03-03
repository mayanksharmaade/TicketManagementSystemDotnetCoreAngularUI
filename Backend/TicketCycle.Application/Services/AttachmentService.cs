using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Interfaces;

namespace TicketCycle.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly string _uploadPath;

        public AttachmentService(IAttachmentRepository attachmentRepository, IConfiguration configuration)
        {
            _attachmentRepository = attachmentRepository;
            _uploadPath = configuration["FileStorage:UploadPath"] ?? "Uploads";
        }

        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByTicketAsync(int ticketId)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByTicketAsync(ticketId);
            return attachments.Select(MapToDto);
        }

        public async Task<AttachmentDto> UploadAttachmentAsync(int ticketId, IFormFile file, int uploadedById)
        {
            // Create upload directory if not exists
            var ticketFolder = Path.Combine(_uploadPath, ticketId.ToString());
            Directory.CreateDirectory(ticketFolder);

            // Save file with unique name to avoid conflicts
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(ticketFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                FileName = file.FileName,
                FilePath = filePath,
                ContentType = file.ContentType,
                FileSize = file.Length,
                TicketId = ticketId,
                UploadedById = uploadedById
            };

            await _attachmentRepository.AddAsync(attachment);
            return MapToDto(attachment);
        }

        public async Task DeleteAttachmentAsync(int id)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Attachment {id} not found.");

            // Delete physical file
            if (File.Exists(attachment.FilePath))
                File.Delete(attachment.FilePath);

            await _attachmentRepository.DeleteAsync(attachment);
        }

        public async Task<(byte[] fileBytes, string contentType, string fileName)> DownloadAttachmentAsync(int id)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Attachment {id} not found.");

            if (!File.Exists(attachment.FilePath))
                throw new FileNotFoundException("File not found on server.");

            var fileBytes = await File.ReadAllBytesAsync(attachment.FilePath);
            return (fileBytes, attachment.ContentType, attachment.FileName);
        }

        private static AttachmentDto MapToDto(Attachment a) => new()
        {
            Id = a.Id,
            FileName = a.FileName,
            ContentType = a.ContentType,
            FileSize = a.FileSize,
            FileSizeFormatted = FormatFileSize(a.FileSize),
            TicketId = a.TicketId,
            UploadedBy = a.UploadedBy?.FullName ?? "",
            UploadedAt = a.UploadedAt,
            DownloadUrl = $"/api/attachments/{a.Id}/download"
        };

        private static string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024):F1} MB";
        }
    }
}
