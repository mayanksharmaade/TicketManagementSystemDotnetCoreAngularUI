using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public int ChangedById { get; set; }
        public User? ChangedBy { get; set; }
        public TicketStatus FromStatus { get; set; }
        public TicketStatus ToStatus { get; set; }
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
