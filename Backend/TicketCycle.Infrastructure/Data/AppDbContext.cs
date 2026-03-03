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
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Sprint> Sprints => Set<Sprint>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Subtask> Subtasks => Set<Subtask>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<TestScript> TestScripts => Set<TestScript>();
        public DbSet<TestLog> TestSteps => Set<TestLog>();
        public DbSet<TicketComment> TicketComments => Set<TicketComment>();
        public DbSet<TicketHistory> TicketHistories => Set<TicketHistory>();

        public DbSet<AuditLog> AuditLogs { get; set; }
        //  public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─────────────────────────────────────────────────────────────
            // USER
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.Role)
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // ─────────────────────────────────────────────────────────────
            // PROJECT
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(p => p.Description)
                      .HasMaxLength(1000);

                entity.Property(p => p.Status)
                      .HasConversion<int>()
                      .HasDefaultValue(ProjectStatus.Active);

                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Project → CreatedBy (User) — restrict delete to avoid cascade conflicts
                entity.HasOne(p => p.CreatedBy)
                      .WithMany()
                      .HasForeignKey(p => p.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─────────────────────────────────────────────────────────────
            // SPRINT
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.Goal)
                      .HasMaxLength(500);

                entity.Property(s => s.Status)
                      .HasConversion<int>()
                      .HasDefaultValue(SprintStatus.Planning);

                entity.Property(s => s.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Sprint → Project
                entity.HasOne(s => s.Project)
                      .WithMany(p => p.Sprints)
                      .HasForeignKey(s => s.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ─────────────────────────────────────────────────────────────
            // TICKET
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(t => t.Description)
                      .HasMaxLength(2000);

                entity.Property(t => t.Status)
                      .HasConversion<int>()
                      .HasDefaultValue(TicketStatus.New);

                entity.Property(t => t.Priority)
                      .HasConversion<int>()
                      .HasDefaultValue(TicketPriority.Medium);

                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Ticket → CreatedBy (User)
                entity.HasOne(t => t.CreatedBy)
                      .WithMany()
                      .HasForeignKey(t => t.CreatedById)
                      .OnDelete(DeleteBehavior.NoAction);
               

                // Ticket → AssignedTo (User) — nullable
                entity.HasOne(t => t.AssignedTo)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedToId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);

                // Ticket → Project — nullable
                entity.HasOne(t => t.Project)
                      .WithMany(p => p.Tickets)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);

                // Ticket → Sprint — nullable
                entity.HasOne(t => t.Sprint)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.SprintId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);
            });

            // ─────────────────────────────────────────────────────────────
            // SUBTASK
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Subtask>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Title)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(s => s.Description)
                      .HasMaxLength(1000);

                entity.Property(s => s.IsCompleted)
                      .HasDefaultValue(false);

                entity.Property(s => s.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Subtask → ParentTicket
                entity.HasOne(s => s.ParentTicket)
                      .WithMany(t => t.Subtasks)
                      .HasForeignKey(s => s.ParentTicketId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Subtask → AssignedTo (User) — nullable
                entity.HasOne(s => s.AssignedTo)
                      .WithMany()
                      .HasForeignKey(s => s.AssignedToId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);
            });

            // ─────────────────────────────────────────────────────────────
            // ATTACHMENT
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.FileName)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(a => a.FilePath)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(a => a.ContentType)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.UploadedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Attachment → Ticket
                entity.HasOne(a => a.Ticket)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(a => a.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Attachment → UploadedBy (User)
                entity.HasOne(a => a.UploadedBy)
                      .WithMany()
                      .HasForeignKey(a => a.UploadedById)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ─────────────────────────────────────────────────────────────
            // TEST SCRIPT
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<TestScript>(entity =>
            {
                entity.HasKey(ts => ts.Id);

                entity.Property(ts => ts.Title)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(ts => ts.Description)
                      .HasMaxLength(2000);

                entity.Property(ts => ts.ExpectedResult)
                      .HasMaxLength(2000);

                entity.Property(ts => ts.Status)
                      .HasConversion<int>()
                      .HasDefaultValue(TestScriptStatus.Draft);

                entity.Property(ts => ts.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // TestScript → Ticket
                entity.HasOne(ts => ts.Ticket)
                      .WithMany(t => t.TestScripts)
                      .HasForeignKey(ts => ts.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);

                // TestScript → CreatedBy (User)
                entity.HasOne(ts => ts.CreatedBy)
                      .WithMany()
                      .HasForeignKey(ts => ts.CreatedById)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ─────────────────────────────────────────────────────────────
            // TEST STEP
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<TestLog>(entity =>
            {
                entity.HasKey(ts => ts.Id);

                entity.Property(ts => ts.Action)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(ts => ts.ExpectedResult)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(ts => ts.ActualResult)
                      .HasMaxLength(1000);

                entity.Property(ts => ts.Status)
                      .HasConversion<int>()
                      .HasDefaultValue(TestStepStatus.NotRun);

                // Order steps within a TestScript
                entity.HasIndex(ts => new { ts.TestScriptId, ts.StepNumber })
                      .IsUnique();

                // TestStep → TestScript
                entity.HasOne(ts => ts.TestScript)
                      .WithMany(s => s.Steps)
                      .HasForeignKey(ts => ts.TestScriptId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ─────────────────────────────────────────────────────────────
            // TICKET COMMENT
            // ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<TicketComment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Content)
                      .IsRequired()
                      .HasMaxLength(2000);

                entity.Property(c => c.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // TicketComment → Ticket
                entity.HasOne(c => c.Ticket)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(c => c.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);

               
            });

          
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
