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

            modelBuilder.Entity<Project>()
            .HasMany(p => p.ProjectCandidates)
            .WithOne(pc => pc.Project);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<Person>(p => p.UserId)
                .IsRequired(false); // Makes UserId nullable

            // For DivisionManager
            modelBuilder.Entity<Person>()
                .HasOne(p => p.DivisionManager)
                .WithMany(dm => dm.ManagedPersonsAsDivisionManager)
                .HasForeignKey(p => p.DivisionManagerId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Restrict); 

            // For TalentManager
            modelBuilder.Entity<Person>()
                .HasOne(p => p.TalentManager)
                .WithMany(tm => tm.ManagedPersonsAsTalentManager)
                .HasForeignKey(p => p.TalentManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); 

            // For Head
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Head)
                .WithMany(h => h.ManagedPersonsAsHead)
                .HasForeignKey(p => p.HeadId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Restrict);
            
            // For DeputyHead
            modelBuilder.Entity<Person>()
                .HasOne(p => p.DeputyHead)
                .WithMany(dh => dh.ManagedPersonsAsDeputyHead)
                .HasForeignKey(p => p.DeputyHeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Connection between Person and SalaryReview
            modelBuilder.Entity<SalaryReview>()
                .HasOne(sr => sr.Person)
                .WithMany(p => p.SalaryReviews)
                .HasForeignKey(sr => sr.PersonId);

            modelBuilder.Entity<Person>()
                .Property(p => p.Comments)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.Location)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.TechStack)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.Stream)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.TMAware)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.PlannedAssignment)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Person>()
                .Property(p => p.Level)
                .HasColumnType("TEXT");

            // Connection between SalaryReviewHistory and Person
            modelBuilder.Entity<SalaryReviewHistory>()
                .HasOne(srh => srh.Person)
                .WithMany() // Assuming there's no navigation property on Person for history records
                .HasForeignKey(srh => srh.PersonId)
                .OnDelete(DeleteBehavior.SetNull);  

            modelBuilder.Entity<ManagementHierarchy>()
                .HasKey(mh => mh.Id);

            modelBuilder.Entity<ManagementHierarchy>()
                .HasMany(mh => mh.Subordinates)
                .WithOne() 
                .HasForeignKey(mh => mh.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ProjectCandidate> ProjectCandidates { get; set; }
        public DbSet<SalaryReview> SalaryReviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SalaryReviewHistory> SalaryReviewHistories { get; set; }  // DbSet for SalaryReviewHistory
        public DbSet<SalaryReviewRequest> SalaryReviewRequests { get; set; }
        public DbSet<ManagementHierarchy> ManagementHierarchies { get; set; }
    }
}
