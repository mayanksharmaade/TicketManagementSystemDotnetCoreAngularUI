using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetDevelopersAsync();
        Task<IEnumerable<UserDto>> GetTestersAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
