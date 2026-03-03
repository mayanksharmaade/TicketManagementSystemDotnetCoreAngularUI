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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllWithDetailsAsync();
            return projects.Select(MapToDto);
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetProjectWithDetailsAsync(id);
            return project == null ? null : MapToDto(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto, int managerId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedById = managerId,
                Status = ProjectStatus.Active
            };

            await _projectRepository.AddAsync(project);
            var created = await _projectRepository.GetProjectWithDetailsAsync(project.Id);
            return MapToDto(created!);
        }

        public async Task<ProjectDto> UpdateProjectAsync(int id, UpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Project {id} not found.");

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.EndDate = dto.EndDate;
            project.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.UpdateAsync(project);
            var updated = await _projectRepository.GetProjectWithDetailsAsync(id);
            return MapToDto(updated!);
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Project {id} not found.");
            await _projectRepository.DeleteAsync(project);
        }

        // ─── Mapping ──────────────────────────────────────────────
        private static ProjectDto MapToDto(Project p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status.ToString(),
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CreatedBy = p.CreatedBy?.FullName ?? "",
            CreatedAt = p.CreatedAt,
            TotalTickets = p.Tickets.Count,
            TotalSprints = p.Sprints.Count,
            Sprints = p.Sprints.Select(s => new SprintDto
            {
                Id = s.Id,
                Name = s.Name,
                Goal = s.Goal,
                Status = s.Status.ToString(),
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                ProjectId = s.ProjectId,
                TotalTickets = s.Tickets.Count
            }).ToList()
        };
    }
}


