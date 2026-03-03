using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;

namespace TicketCycle.Core.Interfaces
{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        // Get all attachments for a ticket
        Task<IEnumerable<Attachment>> GetAttachmentsByTicketAsync(int ticketId);

        // Get attachment with uploader details
        Task<Attachment?> GetAttachmentWithDetailsAsync(int attachmentId);

        // Delete all attachments for a ticket
        Task DeleteByTicketAsync(int ticketId);
    }
}
