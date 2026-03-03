using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;

namespace TicketCycle.Application.Interfaces
{
    public interface ITestScriptService
    {
        Task<IEnumerable<TestScriptDto>> GetTestScriptsByTicketAsync(int ticketId);
        Task<TestScriptDto?> GetTestScriptByIdAsync(int id);
        Task<TestScriptDto> CreateTestScriptAsync(CreateTestScriptDto dto, int createdById);
        Task<TestScriptDto> UpdateTestScriptAsync(int id, UpdateTestScriptDto dto);
        Task DeleteTestScriptAsync(int id);
        Task<TestScriptDto> AddStepAsync(int testScriptId, CreateTestLogDto dto);
        Task<TestScriptDto> UpdateStepResultAsync(int testScriptId, int stepId, UpdateTestLogDto dto);
        Task DeleteStepAsync(int testScriptId, int stepId);
    }
}
