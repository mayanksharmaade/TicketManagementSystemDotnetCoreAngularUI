using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Core.Interfaces
{
    public interface ITestScriptRepository : IGenericRepository<TestScript>
    {
        // Get all test scripts for a ticket
        Task<IEnumerable<TestScript>> GetTestScriptsByTicketAsync(int ticketId);

        // Get test script with all its steps
        Task<TestScript?> GetTestScriptWithStepsAsync(int testScriptId);

        // Add a step to a test script
        Task AddStepAsync(TestLog step);

        // Update a test step result
        Task UpdateStepAsync(TestLog step);

        // Get test scripts by status
        Task<IEnumerable<TestScript>> GetTestScriptsByStatusAsync(TestScriptStatus status);
    }
}
