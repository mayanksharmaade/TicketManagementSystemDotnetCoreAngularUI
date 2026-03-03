using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface ISubtaskService
    {
        Task<IEnumerable<SubtaskDto>> GetSubtasksByTicketAsync(int ticketId);
        Task<SubtaskDto?> GetSubtaskByIdAsync(int id);
        Task<SubtaskDto> CreateSubtaskAsync(CreateSubtaskDto dto);
        Task<SubtaskDto> UpdateSubtaskAsync(int id, UpdateSubtaskDto dto);
        Task DeleteSubtaskAsync(int id);
        Task<SubtaskDto> ToggleCompletionAsync(int id);
        Task<int> GetCompletionPercentageAsync(int ticketId);
    }
}
