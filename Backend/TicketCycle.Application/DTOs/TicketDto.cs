using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketCycle.Application.DTOs
{
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
}
