using Microsoft.EntityFrameworkCore;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;
using TicketCycle.Infrastructure.Data;

namespace TicketCycle.Infrastructure.Repositories
{
  
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role) =>
            await _context.Users
                .Where(u => u.Role == role && u.IsActive)
                .ToListAsync();

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users
                .AnyAsync(u => u.Email == email.ToLower());
    }
}
