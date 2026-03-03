using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class TestScript
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public TestScriptStatus Status { get; set; } = TestScriptStatus.Draft;

        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public int CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<TestLog> Steps { get; set; } = new List<TestLog>();
    }
}
