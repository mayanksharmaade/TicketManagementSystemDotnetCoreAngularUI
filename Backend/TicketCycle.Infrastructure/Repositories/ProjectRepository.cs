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
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetAllWithDetailsAsync() =>
            await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Sprints)
                .Include(p => p.Tickets)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

        public async Task<Project?> GetProjectWithDetailsAsync(int projectId) =>
            await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Sprints).ThenInclude(s => s.Tickets)
                .Include(p => p.Tickets).ThenInclude(t => t.AssignedTo)
                .FirstOrDefaultAsync(p => p.Id == projectId);

        public async Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId) =>
            await _context.Projects
                .Include(p => p.Sprints)
                .Include(p => p.Tickets)
                .Where(p => p.CreatedById == managerId)
                .ToListAsync();

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status) =>
            await _context.Projects
                .Include(p => p.Sprints)
                .Where(p => p.Status == status)
                .ToListAsync();
    }

}
