using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;

namespace TicketCycle.Application.Services
{
    public class TestScriptService : ITestScriptService
    {
        private readonly ITestScriptRepository _testScriptRepository;

        public TestScriptService(ITestScriptRepository testScriptRepository)
        {
            _testScriptRepository = testScriptRepository;
        }

        public async Task<IEnumerable<TestScriptDto>> GetTestScriptsByTicketAsync(int ticketId)
        {
            var scripts = await _testScriptRepository.GetTestScriptsByTicketAsync(ticketId);
            return scripts.Select(MapToDto);
        }

        public async Task<TestScriptDto?> GetTestScriptByIdAsync(int id)
        {
            var script = await _testScriptRepository.GetTestScriptWithStepsAsync(id);
            return script == null ? null : MapToDto(script);
        }

        public async Task<TestScriptDto> CreateTestScriptAsync(CreateTestScriptDto dto, int createdById)
        {
            var script = new TestScript
            {
                Title = dto.Title,
                Description = dto.Description,
                ExpectedResult = dto.ExpectedResult,
                TicketId = dto.TicketId,
                CreatedById = createdById,
                Status = TestScriptStatus.Draft
            };

            await _testScriptRepository.AddAsync(script);
            var created = await _testScriptRepository.GetTestScriptWithStepsAsync(script.Id);
            return MapToDto(created!);
        }

        public async Task<TestScriptDto> UpdateTestScriptAsync(int id, UpdateTestScriptDto dto)
        {
            var script = await _testScriptRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"TestScript {id} not found.");

            script.Title = dto.Title;
            script.Description = dto.Description;
            script.ExpectedResult = dto.ExpectedResult;
            script.Status = dto.Status;
            script.UpdatedAt = DateTime.UtcNow;

            await _testScriptRepository.UpdateAsync(script);
            var updated = await _testScriptRepository.GetTestScriptWithStepsAsync(id);
            return MapToDto(updated!);
        }

        public async Task DeleteTestScriptAsync(int id)
        {
            var script = await _testScriptRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"TestScript {id} not found.");
            await _testScriptRepository.DeleteAsync(script);
        }

        public async Task<TestScriptDto> AddStepAsync(int testScriptId, CreateTestLogDto dto)
        {
            var script = await _testScriptRepository.GetTestScriptWithStepsAsync(testScriptId)
                ?? throw new KeyNotFoundException($"TestScript {testScriptId} not found.");

            var step = new TestLog
            {
                StepNumber = script.Steps.Count + 1,
                Action = dto.Action,
                ExpectedResult = dto.ExpectedResult,
                TestScriptId = testScriptId,
                Status = TestStepStatus.NotRun
            };

            await _testScriptRepository.AddStepAsync(step);
            var updated = await _testScriptRepository.GetTestScriptWithStepsAsync(testScriptId);
            return MapToDto(updated!);
        }

        public async Task<TestScriptDto> UpdateStepResultAsync(int testScriptId, int stepId, UpdateTestLogDto dto)
        {
            var script = await _testScriptRepository.GetTestScriptWithStepsAsync(testScriptId)
                ?? throw new KeyNotFoundException($"TestScript {testScriptId} not found.");

            var step = script.Steps.FirstOrDefault(s => s.Id == stepId)
                ?? throw new KeyNotFoundException($"Step {stepId} not found.");

            step.ActualResult = dto.ActualResult;
            step.Status = dto.Status;
            await _testScriptRepository.UpdateStepAsync(step);

            // Auto update script status based on steps
            var updatedScript = await _testScriptRepository.GetTestScriptWithStepsAsync(testScriptId);
            if (updatedScript!.Steps.All(s => s.Status == TestStepStatus.Passed))
                updatedScript.Status = TestScriptStatus.Passed;
            else if (updatedScript.Steps.Any(s => s.Status == TestStepStatus.Failed))
                updatedScript.Status = TestScriptStatus.Failed;

            await _testScriptRepository.UpdateAsync(updatedScript);
            return MapToDto(updatedScript);
        }

        public async Task DeleteStepAsync(int testScriptId, int stepId)
        {
            var script = await _testScriptRepository.GetTestScriptWithStepsAsync(testScriptId)
                ?? throw new KeyNotFoundException($"TestScript {testScriptId} not found.");

            var step = script.Steps.FirstOrDefault(s => s.Id == stepId)
                ?? throw new KeyNotFoundException($"Step {stepId} not found.");

            await _testScriptRepository.DeleteAsync(script); // will cascade
        }

        private static TestScriptDto MapToDto(TestScript ts) => new()
        {
            Id = ts.Id,
            Title = ts.Title,
            Description = ts.Description,
            ExpectedResult = ts.ExpectedResult,
            Status = ts.Status.ToString(),
            TicketId = ts.TicketId,
            CreatedBy = ts.CreatedBy?.FullName ?? "",
            CreatedAt = ts.CreatedAt,
            TotalSteps = ts.Steps.Count,
            PassedSteps = ts.Steps.Count(s => s.Status == TestStepStatus.Passed),
            FailedSteps = ts.Steps.Count(s => s.Status == TestStepStatus.Failed),
            Steps = ts.Steps.OrderBy(s => s.StepNumber).Select(s => new TestLogDto
            {
                Id = s.Id,
                StepNumber = s.StepNumber,
                Action = s.Action,
                ExpectedResult = s.ExpectedResult,
                ActualResult = s.ActualResult,
                Status = s.Status.ToString()
            }).ToList()
        };
    }
}
