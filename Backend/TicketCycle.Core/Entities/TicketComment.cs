using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

  
}
