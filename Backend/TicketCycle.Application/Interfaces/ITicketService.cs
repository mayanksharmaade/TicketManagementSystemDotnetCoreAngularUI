using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, int createdById);
        Task<TicketDto?> GetTicketByIdAsync(int id);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<IEnumerable<TicketDto>> GetMyTicketsAsync(int userId);
        Task<TicketDto> UpdateTicketStatusAsync(int ticketId, UpdateTicketStatusDto dto, int changedById);
        Task<TicketDto> AssignTicketAsync(int ticketId, AssignTicketDto dto, int managerId);
        Task AddCommentAsync(int ticketId, AddCommentDto dto, int userId);
        Task<DashboardDto> GetManagerDashboardAsync();
        Task<DashboardDto> GetDeveloperDashboardAsync(int userId);
        Task<DashboardDto> GetTesterDashboardAsync(int userId);
    }
}
