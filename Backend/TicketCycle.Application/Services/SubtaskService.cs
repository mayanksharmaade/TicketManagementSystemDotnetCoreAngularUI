using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Interfaces;

namespace TicketCycle.Application.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly ISubtaskRepository _subtaskRepository;

        public SubtaskService(ISubtaskRepository subtaskRepository)
        {
            _subtaskRepository = subtaskRepository;
        }

        public async Task<IEnumerable<SubtaskDto>> GetSubtasksByTicketAsync(int ticketId)
        {
            var subtasks = await _subtaskRepository.GetSubtasksByTicketAsync(ticketId);
            return subtasks.Select(MapToDto);
        }

        public async Task<SubtaskDto?> GetSubtaskByIdAsync(int id)
        {
            var subtask = await _subtaskRepository.GetByIdAsync(id);
            return subtask == null ? null : MapToDto(subtask);
        }

        public async Task<SubtaskDto> CreateSubtaskAsync(CreateSubtaskDto dto)
        {
            var subtask = new Subtask
            {
                Title = dto.Title,
                Description = dto.Description,
                ParentTicketId = dto.ParentTicketId,
                AssignedToId = dto.AssignedToId,
                IsCompleted = false
            };

            await _subtaskRepository.AddAsync(subtask);
            return MapToDto(subtask);
        }

        public async Task<SubtaskDto> UpdateSubtaskAsync(int id, UpdateSubtaskDto dto)
        {
            var subtask = await _subtaskRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Subtask {id} not found.");

            subtask.Title = dto.Title;
            subtask.Description = dto.Description;
            subtask.IsCompleted = dto.IsCompleted;
            subtask.AssignedToId = dto.AssignedToId;

            if (dto.IsCompleted && subtask.CompletedAt == null)
                subtask.CompletedAt = DateTime.UtcNow;
            else if (!dto.IsCompleted)
                subtask.CompletedAt = null;

            await _subtaskRepository.UpdateAsync(subtask);
            return MapToDto(subtask);
        }

        public async Task DeleteSubtaskAsync(int id)
        {
            var subtask = await _subtaskRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Subtask {id} not found.");
            await _subtaskRepository.DeleteAsync(subtask);
        }

        public async Task<SubtaskDto> ToggleCompletionAsync(int id)
        {
            var subtask = await _subtaskRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Subtask {id} not found.");

            subtask.IsCompleted = !subtask.IsCompleted;
            subtask.CompletedAt = subtask.IsCompleted ? DateTime.UtcNow : null;

            await _subtaskRepository.UpdateAsync(subtask);
            return MapToDto(subtask);
        }

        public async Task<int> GetCompletionPercentageAsync(int ticketId)
            => await _subtaskRepository.GetCompletionPercentageAsync(ticketId);

        private static SubtaskDto MapToDto(Subtask s) => new()
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            IsCompleted = s.IsCompleted,
            ParentTicketId = s.ParentTicketId,
            AssignedToId = s.AssignedToId,
            AssigneeName = s.AssignedTo?.FullName,
            CreatedAt = s.CreatedAt,
            CompletedAt = s.CompletedAt
        };
    }
}
