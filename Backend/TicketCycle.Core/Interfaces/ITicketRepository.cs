using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(TicketStatus status);
        Task<IEnumerable<Ticket>> GetTicketsByAssigneeAsync(int userId);
        Task<IEnumerable<Ticket>> GetTicketsByCreatorAsync(int userId);
        Task<Ticket?> GetTicketWithDetailsAsync(int ticketId);
        Task<Dictionary<TicketStatus, int>> GetStatusCountsAsync();
        Task<Dictionary<TicketStatus, int>> GetStatusCountsByUserAsync(int userId);
        Task AddCommentAsync(TicketComment comment);
        Task AddHistoryAsync(TicketHistory history);
        Task<IEnumerable<TicketHistory>> GetHistoryByTicketAsync(int ticketId);
    }
}
