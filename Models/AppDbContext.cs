using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectCandidate>()
                .HasKey(pc => new { pc.ProjectId, pc.PersonId });

            modelBuilder.Entity<ProjectCandidate>()
                .HasOne(pc => pc.Project)
                .WithMany(p => p.ProjectCandidates)
                .HasForeignKey(pc => pc.ProjectId);

            modelBuilder.Entity<ProjectCandidate>()
                .HasOne(pc => pc.Person)
                .WithMany(p => p.ProjectCandidates)
                .HasForeignKey(pc => pc.PersonId);
        }
        
        public DbSet<Project> Projects { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ProjectCandidate> ProjectCandidates { get; set; } // Добавьте эту строку
    }
}
