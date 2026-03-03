using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Enums;

namespace TicketCycle.Application.DTOs
{
    public class TestScriptDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TicketId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<TestLogDto> Steps { get; set; } = new();
        public int TotalSteps { get; set; }
        public int PassedSteps { get; set; }
        public int FailedSteps { get; set; }
    }

    public class CreateTestScriptDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public int TicketId { get; set; }
    }

    public class UpdateTestScriptDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public TestScriptStatus Status { get; set; }
    }
}
