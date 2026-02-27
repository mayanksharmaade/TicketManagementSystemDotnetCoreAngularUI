using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> EmailExistsAsync(string email);
    }
}
