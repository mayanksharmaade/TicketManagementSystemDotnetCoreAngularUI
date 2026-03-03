using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Application.DTOs
{
    public class TestLogDto
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public string? ActualResult { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateTestLogDto
    {
        public string Action { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public int TestScriptId { get; set; }
    }

    public class UpdateTestLogDto
    {
        public string? ActualResult { get; set; }
        public TestStepStatus Status { get; set; }
    }
}
