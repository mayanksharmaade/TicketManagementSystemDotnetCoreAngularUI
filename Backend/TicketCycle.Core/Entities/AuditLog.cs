using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketCycle.Core.Entities
{
    public class AuditLog { public int Id { get; set; } 
        public string EntityName { get; set; }
        public int EntityId { get; set; } 
        public string ActionType { get; set; } 
        public string OldValues { get; set; } 
        public string NewValues { get; set; }
        public int ChangedBy { get; set; } 
        public DateTime ChangedOn { get; set; }
        public string Description { get; set; } }
}
