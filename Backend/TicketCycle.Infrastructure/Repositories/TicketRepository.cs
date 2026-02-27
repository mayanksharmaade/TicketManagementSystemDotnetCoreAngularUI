using Microsoft.EntityFrameworkCore;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;
using TicketCycle.Infrastructure.Data;

namespace TicketCycle.Infrastructure.Repositories
{
    // ─────────────────────────────────────────────────────────────
    // Infrastructure Layer - Ticket Repository
    // ONLY handles data access - no business logic here
    // ─────────────────────────────────────────────────────────────
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(TicketStatus status) =>
            await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetTicketsByAssigneeAsync(int userId) =>
            await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetTicketsByCreatorAsync(int userId) =>
            await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.CreatedById == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<Ticket?> GetTicketWithDetailsAsync(int ticketId) =>
            await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments).ThenInclude(c => c.User)
                .Include(t => t.Histories).ThenInclude(h => h.ChangedBy)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

        public async Task<Dictionary<TicketStatus, int>> GetStatusCountsAsync() =>
            await _context.Tickets
                .GroupBy(t => t.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

        public async Task<Dictionary<TicketStatus, int>> GetStatusCountsByUserAsync(int userId) =>
            await _context.Tickets
                .Where(t => t.AssignedToId == userId)
                .GroupBy(t => t.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

        public async Task AddCommentAsync(TicketComment comment)
        {
            await _context.TicketComments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task AddHistoryAsync(TicketHistory history)
        {
            await _context.TicketHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketHistory>> GetHistoryByTicketAsync(int ticketId) =>
            await _context.TicketHistories
                .Include(h => h.ChangedBy)
                .Where(h => h.TicketId == ticketId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
    }
}
