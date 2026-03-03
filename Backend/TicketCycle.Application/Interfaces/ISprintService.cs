using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface ISprintService
    {
        Task<IEnumerable<SprintDto>> GetSprintsByProjectAsync(int projectId);
        Task<SprintDto?> GetSprintByIdAsync(int id);
        Task<SprintDto> CreateSprintAsync(CreateSprintDto dto);
        Task<SprintDto> UpdateSprintAsync(int id, UpdateSprintDto dto);
        Task DeleteSprintAsync(int id);
        Task<SprintDto> StartSprintAsync(int id);
        Task<SprintDto> CompleteSprintAsync(int id);
        Task AddTicketToSprintAsync(int sprintId, int ticketId);
        Task RemoveTicketFromSprintAsync(int sprintId, int ticketId);
    }
}
