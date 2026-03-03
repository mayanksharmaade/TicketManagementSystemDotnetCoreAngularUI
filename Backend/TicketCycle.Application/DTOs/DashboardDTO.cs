using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketCycle.Application.DTOs
{
    public class DashboardDto
    {
        public Dictionary<string, int> StatusCounts { get; set; } = new();
        public List<TicketDto> RecentTickets { get; set; } = new();
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
    }
}
