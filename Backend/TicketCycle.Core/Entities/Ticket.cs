using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.New;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        // Who created and who is assigned
        public int CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }

        // NEW: Project and Sprint
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
        public int? SprintId { get; set; }
        public Sprint? Sprint { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        // Navigation
        public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public ICollection<TicketHistory> Histories { get; set; } = new List<TicketHistory>();

        // NEW Navigation
        public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<TestScript> TestScripts { get; set; } = new List<TestScript>();
    }
}
