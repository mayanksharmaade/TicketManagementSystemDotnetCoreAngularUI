using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;

namespace TicketCycle.Core.Interfaces
{
    public interface ISubtaskRepository : IGenericRepository<Subtask>
    {
        // Get all subtasks for a ticket
        Task<IEnumerable<Subtask>> GetSubtasksByTicketAsync(int ticketId);

        // Get subtasks assigned to a user
        Task<IEnumerable<Subtask>> GetSubtasksByAssigneeAsync(int userId);

        // Get completion percentage for a ticket
        Task<int> GetCompletionPercentageAsync(int ticketId);

        // Mark subtask as complete
        Task MarkAsCompletedAsync(int subtaskId);
    }
}
