using TicketCycle.Application.DTOs;
using TicketCycle.Application.Interfaces;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;
using TicketCycle.Core.Interfaces;

namespace TicketCycle.Application.Services
{
    // ─────────────────────────────────────────────────────────────
    // Business Logic Layer - Ticket Service
    // This layer ONLY knows about Core (Entities + Repository Interfaces)
    // It does NOT know about EF Core, SQL Server, or HTTP
    // ─────────────────────────────────────────────────────────────
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, int createdById)
        {
            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                CreatedById = createdById,
                AssignedToId = dto.AssignedToId,
                Status = dto.AssignedToId.HasValue ? TicketStatus.Assigned : TicketStatus.New
            };

            await _ticketRepository.AddAsync(ticket);

            // Log history entry
            await _ticketRepository.AddHistoryAsync(new TicketHistory
            {
                TicketId = ticket.Id,
                ChangedById = createdById,
                FromStatus = TicketStatus.New,
                ToStatus = ticket.Status,
                Note = "Ticket created"
            });

            var created = await _ticketRepository.GetTicketWithDetailsAsync(ticket.Id);
            return MapToDto(created!);
        }

        public async Task<TicketDto?> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTicketWithDetailsAsync(id);
            return ticket == null ? null : MapToDto(ticket);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<TicketDto>> GetMyTicketsAsync(int userId)
        {
            var tickets = await _ticketRepository.GetTicketsByAssigneeAsync(userId);
            return tickets.Select(MapToDto);
        }

        public async Task<TicketDto> UpdateTicketStatusAsync(int ticketId, UpdateTicketStatusDto dto, int changedById)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var oldStatus = ticket.Status;
            ticket.Status = dto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);

            await _ticketRepository.AddHistoryAsync(new TicketHistory
            {
                TicketId = ticket.Id,
                ChangedById = changedById,
                FromStatus = oldStatus,
                ToStatus = dto.Status,
                Note = dto.Note
            });

            var updated = await _ticketRepository.GetTicketWithDetailsAsync(ticket.Id);
            return MapToDto(updated!);
        }

        public async Task<TicketDto> AssignTicketAsync(int ticketId, AssignTicketDto dto, int managerId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

            var oldStatus = ticket.Status;
            ticket.AssignedToId = dto.AssignedToId;
            ticket.Status = TicketStatus.Assigned;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);

            await _ticketRepository.AddHistoryAsync(new TicketHistory
            {
                TicketId = ticket.Id,
                ChangedById = managerId,
                FromStatus = oldStatus,
                ToStatus = TicketStatus.Assigned,
                Note = "Ticket assigned to developer"
            });

            var updated = await _ticketRepository.GetTicketWithDetailsAsync(ticket.Id);
            return MapToDto(updated!);
        }

        public async Task AddCommentAsync(int ticketId, AddCommentDto dto, int userId)
        {
            var comment = new TicketComment
            {
                TicketId = ticketId,
                UserId = userId,
                Content = dto.Content
            };
            await _ticketRepository.AddCommentAsync(comment);
        }

        public async Task<DashboardDto> GetManagerDashboardAsync()
        {
            var statusCounts = await _ticketRepository.GetStatusCountsAsync();
            var allTickets = await _ticketRepository.GetAllAsync();
            var ticketList = allTickets.ToList();

            return new DashboardDto
            {
                StatusCounts = statusCounts.ToDictionary(k => k.Key.ToString(), v => v.Value),
                TotalTickets = ticketList.Count,
                OpenTickets = ticketList.Count(t => t.Status != TicketStatus.Closed),
                ClosedTickets = ticketList.Count(t => t.Status == TicketStatus.Closed),
                RecentTickets = ticketList.OrderByDescending(t => t.CreatedAt).Take(5).Select(MapToDto).ToList()
            };
        }

        public async Task<DashboardDto> GetDeveloperDashboardAsync(int userId)
        {
            var statusCounts = await _ticketRepository.GetStatusCountsByUserAsync(userId);
            var myTickets = (await _ticketRepository.GetTicketsByAssigneeAsync(userId)).ToList();

            return new DashboardDto
            {
                StatusCounts = statusCounts.ToDictionary(k => k.Key.ToString(), v => v.Value),
                TotalTickets = myTickets.Count,
                OpenTickets = myTickets.Count(t => t.Status != TicketStatus.Closed),
                ClosedTickets = myTickets.Count(t => t.Status == TicketStatus.Closed),
                RecentTickets = myTickets.OrderByDescending(t => t.CreatedAt).Take(5).Select(MapToDto).ToList()
            };
        }

        public async Task<DashboardDto> GetTesterDashboardAsync(int userId)
        {
            var testingTickets = (await _ticketRepository.GetTicketsByStatusAsync(TicketStatus.Testing)).ToList();
            var resolvedTickets = (await _ticketRepository.GetTicketsByStatusAsync(TicketStatus.Resolved)).ToList();
            var allForTester = testingTickets.Concat(resolvedTickets).ToList();

            return new DashboardDto
            {
                StatusCounts = new Dictionary<string, int>
                {
                    { TicketStatus.Testing.ToString(), testingTickets.Count },
                    { TicketStatus.Resolved.ToString(), resolvedTickets.Count }
                },
                TotalTickets = allForTester.Count,
                OpenTickets = testingTickets.Count,
                ClosedTickets = resolvedTickets.Count,
                RecentTickets = allForTester.OrderByDescending(t => t.CreatedAt).Take(5).Select(MapToDto).ToList()
            };
        }

        // ─── Private Mapping Helper ───────────────────────────────
        private static TicketDto MapToDto(Ticket t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            StatusId = (int)t.Status,
            Priority = t.Priority.ToString(),
            PriorityId = (int)t.Priority,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate,
            CreatedBy = t.CreatedBy == null ? null : new UserDto
            {
                Id = t.CreatedBy.Id,
                FullName = t.CreatedBy.FullName,
                Email = t.CreatedBy.Email,
                Role = t.CreatedBy.Role.ToString()
            },
            AssignedTo = t.AssignedTo == null ? null : new UserDto
            {
                Id = t.AssignedTo.Id,
                FullName = t.AssignedTo.FullName,
                Email = t.AssignedTo.Email,
                Role = t.AssignedTo.Role.ToString()
            },
            Comments = t.Comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                AuthorName = c.User?.FullName ?? "",
                CreatedAt = c.CreatedAt
            }).ToList(),
            History = t.Histories.OrderByDescending(h => h.ChangedAt).Select(h => new HistoryDto
            {
                FromStatus = h.FromStatus.ToString(),
                ToStatus = h.ToStatus.ToString(),
                ChangedBy = h.ChangedBy?.FullName ?? "",
                Note = h.Note,
                ChangedAt = h.ChangedAt
            }).ToList()
        };
    }
}
