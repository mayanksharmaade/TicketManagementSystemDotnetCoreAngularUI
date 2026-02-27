using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;

namespace TicketCycle.Application.Services
{
    // ─────────────────────────────────────────────────────────────
    // Business Logic Layer - User Service
    // ─────────────────────────────────────────────────────────────
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetDevelopersAsync()
        {
            var developers = await _userRepository.GetUsersByRoleAsync(UserRole.Developer);
            return developers.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetTestersAsync()
        {
            var testers = await _userRepository.GetUsersByRoleAsync(UserRole.Tester);
            return testers.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        private static UserDto MapToDto(Core.Entities.User u) => new()
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role.ToString()
        };
    }
}
