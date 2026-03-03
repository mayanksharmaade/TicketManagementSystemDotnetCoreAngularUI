using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Application.DTOs
{
    public class SprintDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Goal { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TotalTickets { get; set; }
        public List<TicketDto> Tickets { get; set; } = new();
    }

    public class CreateSprintDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Goal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
    }

    public class UpdateSprintDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Goal { get; set; }
        public SprintStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
