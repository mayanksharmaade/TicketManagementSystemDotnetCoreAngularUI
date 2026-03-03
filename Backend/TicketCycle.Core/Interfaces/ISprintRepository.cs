using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Interfaces
{
    public interface ISprintRepository : IGenericRepository<Sprint>
    {
        // Get all sprints for a project
        Task<IEnumerable<Sprint>> GetSprintsByProjectAsync(int projectId);

        // Get sprint with all its tickets
        Task<Sprint?> GetSprintWithTicketsAsync(int sprintId);

        // Get active sprint for a project
        Task<Sprint?> GetActiveSprintAsync(int projectId);

        // Get sprints by status
        Task<IEnumerable<Sprint>> GetSprintsByStatusAsync(SprintStatus status);
    }
}
