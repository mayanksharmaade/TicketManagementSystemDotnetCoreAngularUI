using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        // Get all projects with their sprints and ticket counts
        Task<IEnumerable<Project>> GetAllWithDetailsAsync();

        // Get single project with all sprints and tickets
        Task<Project?> GetProjectWithDetailsAsync(int projectId);

        // Get all projects created by a specific manager
        Task<IEnumerable<Project>> GetProjectsByManagerAsync(int managerId);

        // Get projects by status
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);
    }
}

