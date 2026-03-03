using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Entities
{
    public class TestLog
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public string? ActualResult { get; set; }
        public TestStepStatus Status { get; set; } = TestStepStatus.NotRun;

        public int TestScriptId { get; set; }
        public TestScript? TestScript { get; set; }
    }
}
