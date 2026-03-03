using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;
using TicketCycle.Infrastructure.Data;

namespace TicketCycle.Infrastructure.Repositories
{
    public class TestScriptRepository : GenericRepository<TestScript>, ITestScriptRepository
    {
        public TestScriptRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TestScript>> GetTestScriptsByTicketAsync(int ticketId) =>
            await _context.TestScripts
                .Include(ts => ts.CreatedBy)
                .Include(ts => ts.Steps)
                .Where(ts => ts.TicketId == ticketId)
                .ToListAsync();

        public async Task<TestScript?> GetTestScriptWithStepsAsync(int testScriptId) =>
            await _context.TestScripts
                .Include(ts => ts.CreatedBy)
                .Include(ts => ts.Steps)
                .FirstOrDefaultAsync(ts => ts.Id == testScriptId);

        public async Task AddStepAsync(TestLog step)
        {
            await _context.TestSteps.AddAsync(step);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStepAsync(TestLog step)
        {
            _context.TestSteps.Update(step);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TestScript>> GetTestScriptsByStatusAsync(TestScriptStatus status) =>
            await _context.TestScripts
                .Include(ts => ts.Steps)
                .Where(ts => ts.Status == status)
                .ToListAsync();

     
    }
}
