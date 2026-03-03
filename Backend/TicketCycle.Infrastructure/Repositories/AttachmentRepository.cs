using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Interfaces;
using TicketCycle.Infrastructure.Data;

namespace TicketCycle.Infrastructure.Repositories
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByTicketAsync(int ticketId) =>
            await _context.Attachments
                .Include(a => a.UploadedBy)
                .Where(a => a.TicketId == ticketId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();

        public async Task<Attachment?> GetAttachmentWithDetailsAsync(int attachmentId) =>
            await _context.Attachments
                .Include(a => a.UploadedBy)
                .FirstOrDefaultAsync(a => a.Id == attachmentId);

        public async Task DeleteByTicketAsync(int ticketId)
        {
            var attachments = await _context.Attachments
                .Where(a => a.TicketId == ticketId)
                .ToListAsync();
            _context.Attachments.RemoveRange(attachments);
            await _context.SaveChangesAsync();
        }
    }
}
