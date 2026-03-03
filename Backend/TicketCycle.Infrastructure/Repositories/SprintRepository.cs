using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;
using TicketCycle.Infrastructure.Data;

namespace TicketCycle.Infrastructure.Repositories
{
    public class SprintRepository : GenericRepository<Sprint>, ISprintRepository
    {
        public SprintRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Sprint>> GetSprintsByProjectAsync(int projectId) =>
            await _context.Sprints
                .Include(s => s.Project)
                .Include(s => s.Tickets).ThenInclude(t => t.AssignedTo)
                .Where(s => s.ProjectId == projectId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

        public async Task<Sprint?> GetSprintWithTicketsAsync(int sprintId) =>
            await _context.Sprints
                .Include(s => s.Project)
                .Include(s => s.Tickets).ThenInclude(t => t.AssignedTo)
                .Include(s => s.Tickets).ThenInclude(t => t.CreatedBy)
                .FirstOrDefaultAsync(s => s.Id == sprintId);

        public async Task<Sprint?> GetActiveSprintAsync(int projectId) =>
            await _context.Sprints
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Status == SprintStatus.Active);

        public async Task<IEnumerable<Sprint>> GetSprintsByStatusAsync(SprintStatus status) =>
            await _context.Sprints
                .Include(s => s.Project)
                .Where(s => s.Status == status)
                .ToListAsync();
    }
}
