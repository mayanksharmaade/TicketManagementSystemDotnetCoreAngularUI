using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

       

       
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime JoinedOn { get; set; }
        public DateTime? LeftOn { get; set; }
        public int? AllocationPercent { get; set; }

      

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

       

        public DateTime? UpdatedOn { get; set; }
        public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }

}
