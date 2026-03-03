using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class Sprint
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Goal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SprintStatus Status { get; set; } = SprintStatus.Planning;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
