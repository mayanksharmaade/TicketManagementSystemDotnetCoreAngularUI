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
    public class SubtaskRepository : GenericRepository<Subtask>, ISubtaskRepository
    {
        public SubtaskRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Subtask>> GetSubtasksByTicketAsync(int ticketId) =>
            await _context.Subtasks
                .Include(s => s.AssignedTo)
                .Where(s => s.ParentTicketId == ticketId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Subtask>> GetSubtasksByAssigneeAsync(int userId) =>
            await _context.Subtasks
                .Include(s => s.ParentTicket)
                .Where(s => s.AssignedToId == userId)
                .ToListAsync();

        public async Task<int> GetCompletionPercentageAsync(int ticketId)
        {
            var subtasks = await _context.Subtasks
                .Where(s => s.ParentTicketId == ticketId)
                .ToListAsync();

            if (!subtasks.Any()) return 0;

            var completed = subtasks.Count(s => s.IsCompleted);
            return (int)Math.Round((double)completed / subtasks.Count * 100);
        }

        public async Task MarkAsCompletedAsync(int subtaskId)
        {
            var subtask = await _context.Subtasks.FindAsync(subtaskId);
            if (subtask != null)
            {
                subtask.IsCompleted = true;
                subtask.CompletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
