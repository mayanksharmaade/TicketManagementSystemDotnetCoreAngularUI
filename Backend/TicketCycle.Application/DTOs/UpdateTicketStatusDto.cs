using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Application.DTOs
{
    public class UpdateTicketStatusDto
    {
        public TicketStatus Status { get; set; }
        public string? Note { get; set; }
    }
}
