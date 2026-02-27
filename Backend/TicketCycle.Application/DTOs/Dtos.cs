using TicketCycle.Core.Enums;

namespace TicketCycle.Application.DTOs
{
    // ─── Auth DTOs ───────────────────────────────────────────
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }

    // ─── User DTOs ───────────────────────────────────────────
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    // ─── Ticket DTOs ─────────────────────────────────────────
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string Priority { get; set; } = string.Empty;
        public int PriorityId { get; set; }
        public UserDto? CreatedBy { get; set; }
        public UserDto? AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
        public List<HistoryDto> History { get; set; } = new();
    }

    public class CreateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public DateTime? DueDate { get; set; }
        public int? AssignedToId { get; set; }
    }

    public class UpdateTicketStatusDto
    {
        public TicketStatus Status { get; set; }
        public string? Note { get; set; }
    }

    public class AssignTicketDto
    {
        public int AssignedToId { get; set; }
    }

    public class AddCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }

    // ─── Comment & History DTOs ───────────────────────────────
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class HistoryDto
    {
        public string FromStatus { get; set; } = string.Empty;
        public string ToStatus { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; }
    }

    // ─── Dashboard DTOs ───────────────────────────────────────
    public class DashboardDto
    {
        public Dictionary<string, int> StatusCounts { get; set; } = new();
        public List<TicketDto> RecentTickets { get; set; } = new();
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
    }
}
