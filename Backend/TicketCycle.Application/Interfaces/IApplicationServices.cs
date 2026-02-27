using TicketCycle.Application.DTOs;
using TicketCycle.Core.Entities;

namespace TicketCycle.Application.Interfaces
{
    // ─── Auth Service Interface ───────────────────────────────
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        string GenerateJwtToken(User user);
    }

    // ─── Ticket Service Interface ─────────────────────────────
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

    // ─── User Service Interface ───────────────────────────────
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetDevelopersAsync();
        Task<IEnumerable<UserDto>> GetTestersAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
