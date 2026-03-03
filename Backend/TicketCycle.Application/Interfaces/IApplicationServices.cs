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

    
}
