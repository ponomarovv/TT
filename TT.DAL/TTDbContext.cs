using Microsoft.EntityFrameworkCore;
using TT.DAL.Entities;

namespace TT.DAL
{
    public class TTDbContext : DbContext
    {
        public TTDbContext(DbContextOptions<TTDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<TimeEntry> TimeEntries { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.TimeEntries)
                .WithOne(te => te.Project)
                .HasForeignKey(te => te.ProjectId);

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(te => te.Id);
                entity.HasOne(te => te.Project)
                    .WithMany(p => p.TimeEntries)
                    .HasForeignKey(te => te.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<TimeEntry>()
                .HasCheckConstraint("CK_TimeEntry_MinDuration", "DATEDIFF(MINUTE, StartTime, EndTime) >= 15");
        }
    }
}
