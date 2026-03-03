using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketCycle.Application.DTOs
{
    public class SubtaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int ParentTicketId { get; set; }
        public string? AssigneeName { get; set; }
        public int? AssignedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateSubtaskDto
    {

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ParentTicketId { get; set; }
        public int? AssignedToId { get; set; }
    }

    public class UpdateSubtaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int? AssignedToId { get; set; }
    }
}
