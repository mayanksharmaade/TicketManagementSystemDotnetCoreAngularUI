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
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly ITicketRepository _ticketRepository;

        public SprintService(ISprintRepository sprintRepository, ITicketRepository ticketRepository)
        {
            _sprintRepository = sprintRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<SprintDto>> GetSprintsByProjectAsync(int projectId)
        {
            var sprints = await _sprintRepository.GetSprintsByProjectAsync(projectId);
            return sprints.Select(MapToDto);
        }

        public async Task<SprintDto?> GetSprintByIdAsync(int id)
        {
            var sprint = await _sprintRepository.GetSprintWithTicketsAsync(id);
            return sprint == null ? null : MapToDto(sprint);
        }

        public async Task<SprintDto> CreateSprintAsync(CreateSprintDto dto)
        {
            var sprint = new Sprint
            {
                Name = dto.Name,
                Goal = dto.Goal,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ProjectId = dto.ProjectId,
                Status = SprintStatus.Planning
            };

            await _sprintRepository.AddAsync(sprint);
            var created = await _sprintRepository.GetSprintWithTicketsAsync(sprint.Id);
            return MapToDto(created!);
        }

        public async Task<SprintDto> UpdateSprintAsync(int id, UpdateSprintDto dto)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Sprint {id} not found.");

            sprint.Name = dto.Name;
            sprint.Goal = dto.Goal;
            sprint.Status = dto.Status;
            sprint.StartDate = dto.StartDate;
            sprint.EndDate = dto.EndDate;
            sprint.UpdatedAt = DateTime.UtcNow;

            await _sprintRepository.UpdateAsync(sprint);
            var updated = await _sprintRepository.GetSprintWithTicketsAsync(id);
            return MapToDto(updated!);
        }

        public async Task DeleteSprintAsync(int id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Sprint {id} not found.");
            await _sprintRepository.DeleteAsync(sprint);
        }

        public async Task<SprintDto> StartSprintAsync(int id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Sprint {id} not found.");

            // Only one sprint can be active at a time per project
            var activeSprint = await _sprintRepository.GetActiveSprintAsync(sprint.ProjectId);
            if (activeSprint != null && activeSprint.Id != id)
                throw new InvalidOperationException("Another sprint is already active in this project.");

            sprint.Status = SprintStatus.Active;
            sprint.UpdatedAt = DateTime.UtcNow;
            await _sprintRepository.UpdateAsync(sprint);

            var updated = await _sprintRepository.GetSprintWithTicketsAsync(id);
            return MapToDto(updated!);
        }

        public async Task<SprintDto> CompleteSprintAsync(int id)
        {
            var sprint = await _sprintRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Sprint {id} not found.");

            sprint.Status = SprintStatus.Completed;
            sprint.UpdatedAt = DateTime.UtcNow;
            await _sprintRepository.UpdateAsync(sprint);

            var updated = await _sprintRepository.GetSprintWithTicketsAsync(id);
            return MapToDto(updated!);
        }

        public async Task AddTicketToSprintAsync(int sprintId, int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket {ticketId} not found.");

            ticket.SprintId = sprintId;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task RemoveTicketFromSprintAsync(int sprintId, int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket {ticketId} not found.");

            ticket.SprintId = null;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
        }

        // ─── Mapping ──────────────────────────────────────────────
        private static SprintDto MapToDto(Sprint s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            Goal = s.Goal,
            Status = s.Status.ToString(),
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            ProjectId = s.ProjectId,
            ProjectName = s.Project?.Name ?? "",
            CreatedAt = s.CreatedAt,
            TotalTickets = s.Tickets.Count,
            Tickets = s.Tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                StatusId = (int)t.Status,
                Priority = t.Priority.ToString(),
                PriorityId = (int)t.Priority,
                AssignedTo = t.AssignedTo == null ? null : new UserDto
                {
                    Id = t.AssignedTo.Id,
                    FullName = t.AssignedTo.FullName,
                    Email = t.AssignedTo.Email,
                    Role = t.AssignedTo.Role.ToString()
                }
            }).ToList()
        };
    }
}


