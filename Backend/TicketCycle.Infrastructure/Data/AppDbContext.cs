using Microsoft.EntityFrameworkCore;
using TicketCycle.Core.Entities;
using TicketCycle.Core.Enums;

namespace TicketCycle.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── User Configuration ───────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(200);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Role).HasConversion<int>();
            });

            // ─── Ticket Configuration ─────────────────────────────
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(300);
                entity.Property(t => t.Description).IsRequired();
                entity.Property(t => t.Status).HasConversion<int>();
                entity.Property(t => t.Priority).HasConversion<int>();

                entity.HasOne(t => t.CreatedBy)
                    .WithMany(u => u.CreatedTickets)
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedTo)
                    .WithMany(u => u.AssignedTickets)
                    .HasForeignKey(t => t.AssignedToId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ─── Comment Configuration ────────────────────────────
            modelBuilder.Entity<TicketComment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired();
                entity.HasOne(c => c.Ticket)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── History Configuration ────────────────────────────
            modelBuilder.Entity<TicketHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.FromStatus).HasConversion<int>();
                entity.Property(h => h.ToStatus).HasConversion<int>();
                entity.HasOne(h => h.Ticket)
                    .WithMany(t => t.Histories)
                    .HasForeignKey(h => h.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(h => h.ChangedBy)
                    .WithMany(u => u.TicketHistories)
                    .HasForeignKey(h => h.ChangedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Seed Data ────────────────────────────────────────
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin Manager",
                    Email = "manager@ticketcycle.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 2,
                    FullName = "John Developer",
                    Email = "developer@ticketcycle.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Developer@123"),
                    Role = UserRole.Developer,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 3,
                    FullName = "Jane Tester",
                    Email = "tester@ticketcycle.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tester@123"),
                    Role = UserRole.Tester,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
