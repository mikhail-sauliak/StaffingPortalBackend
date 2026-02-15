// using Microsoft.EntityFrameworkCore;
// using StaffingPortalBackend.Models;

// namespace StaffingPortalBackend.Models
// {
//     public class AppDbContext : DbContext
//     {
//         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//         {
//         }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
//                 optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=staffingportaldb;Username=user;Password=password;");
//             }
//         }
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.Entity<ProjectCandidate>()
//                 .HasKey(pc => new { pc.ProjectId, pc.PersonId });

//             modelBuilder.Entity<Project>()
//             .HasMany(p => p.ProjectCandidates)
//             .WithOne(pc => pc.Project);

//             modelBuilder.Entity<User>()
//                 .HasOne(u => u.Person)
//                 .WithOne(p => p.User)
//                 .HasForeignKey<Person>(p => p.UserId)
//                 .IsRequired(false); // UserId nullable

//             // For DivisionManager
//             modelBuilder.Entity<Person>()
//                 .HasOne(p => p.DivisionManager)
//                 .WithMany(dm => dm.ManagedPersonsAsDivisionManager)
//                 .HasForeignKey(p => p.DivisionManagerId)
//                 .IsRequired(false)
//                 .OnDelete(DeleteBehavior.Restrict);

//             // For TalentManager
//             modelBuilder.Entity<Person>()
//                 .HasOne(p => p.TalentManager)
//                 .WithMany(tm => tm.ManagedPersonsAsTalentManager)
//                 .HasForeignKey(p => p.TalentManagerId)
//                 .IsRequired(false)
//                 .OnDelete(DeleteBehavior.Restrict);

//             // For Head
//             modelBuilder.Entity<Person>()
//                 .HasOne(p => p.Head)
//                 .WithMany(h => h.ManagedPersonsAsHead)
//                 .HasForeignKey(p => p.HeadId)
//                 .IsRequired(false)
//                 .OnDelete(DeleteBehavior.Restrict);

//             // For DeputyHead
//             modelBuilder.Entity<Person>()
//                 .HasOne(p => p.DeputyHead)
//                 .WithMany(dh => dh.ManagedPersonsAsDeputyHead)
//                 .HasForeignKey(p => p.DeputyHeadId)
//                 .IsRequired(false)
//                 .OnDelete(DeleteBehavior.Restrict);

//             // between Person and SalaryReview
//             modelBuilder.Entity<SalaryReview>()
//                 .HasOne(sr => sr.Person)
//                 .WithMany(p => p.SalaryReviews)
//                 .HasForeignKey(sr => sr.PersonId);

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.Comments)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.FirstName)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.LastName)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.Location)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.TechStack)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.Stream)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.TMAware)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.PlannedAssignment)
//                 .HasColumnType("TEXT");

//             modelBuilder.Entity<Person>()
//                 .Property(p => p.Level)
//                 .HasColumnType("TEXT");

//             // between SalaryReviewHistory and Person
//             modelBuilder.Entity<SalaryReviewHistory>()
//                 .HasOne(srh => srh.Person)
//                 .WithMany()
//                 .HasForeignKey(srh => srh.PersonId)
//                 .OnDelete(DeleteBehavior.SetNull);

//             modelBuilder.Entity<ManagementHierarchy>()
//                 .HasKey(mh => mh.Id);

//             modelBuilder.Entity<ManagementHierarchy>()
//                 .HasMany(mh => mh.Subordinates)
//                 .WithOne()
//                 .HasForeignKey(mh => mh.ManagerId)
//                 .OnDelete(DeleteBehavior.NoAction);

//             modelBuilder.Entity<QuarterlyDepartmentSalaryChange>()
//                 .HasKey(qdsc => qdsc.Id);
//         }

//         public DbSet<Project> Projects { get; set; }
//         public DbSet<Person> People { get; set; }
//         public DbSet<ProjectCandidate> ProjectCandidates { get; set; }
//         public DbSet<SalaryReview> SalaryReviews { get; set; }
//         public DbSet<User> Users { get; set; }
//         public DbSet<SalaryReviewHistory> SalaryReviewHistories { get; set; }
//         public DbSet<SalaryReviewRequest> SalaryReviewRequests { get; set; }
//         public DbSet<ManagementHierarchy> ManagementHierarchies { get; set; }
//         public DbSet<QuarterlyDepartmentSalaryChange> QuarterlyDepartmentSalaryChanges { get; set; }
//     }
// }

using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StaffingPortalBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),  
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.Entity<ProjectCandidate>()
                .HasKey(pc => new { pc.ProjectId, pc.PersonId });

            modelBuilder.Entity<Project>()
            .HasMany(p => p.ProjectCandidates)
            .WithOne(pc => pc.Project);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<Person>(p => p.UserId)
                .IsRequired(false); 

            modelBuilder.Entity<Person>()
                .HasOne(p => p.DivisionManager)
                .WithMany(dm => dm.ManagedPersonsAsDivisionManager)
                .HasForeignKey(p => p.DivisionManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.TalentManager)
                .WithMany(tm => tm.ManagedPersonsAsTalentManager)
                .HasForeignKey(p => p.TalentManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Head)
                .WithMany(h => h.ManagedPersonsAsHead)
                .HasForeignKey(p => p.HeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.DeputyHead)
                .WithMany(dh => dh.ManagedPersonsAsDeputyHead)
                .HasForeignKey(p => p.DeputyHeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // between Person and SalaryReview
            modelBuilder.Entity<SalaryReview>()
                .HasOne(sr => sr.Person)
                .WithMany(p => p.SalaryReviews)
                .HasForeignKey(sr => sr.PersonId);
            
            modelBuilder.Entity<Person>()
                .Property(p => p.Comments)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.Location)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.TechStack)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.Stream)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.TMAware)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.PlannedAssignment)
                .HasColumnType("text");

            modelBuilder.Entity<Person>()
                .Property(p => p.Level)
                .HasColumnType("text");

            // between SalaryReviewHistory and Person
            modelBuilder.Entity<SalaryReviewHistory>()
                .HasOne(srh => srh.Person)
                .WithMany()
                .HasForeignKey(srh => srh.PersonId)
                .OnDelete(DeleteBehavior.SetNull);

            var hierarchyBuilder = modelBuilder.Entity<ManagementHierarchy>();
            hierarchyBuilder.HasKey(mh => mh.Id);
            hierarchyBuilder.HasIndex(mh => mh.PersonId).IsUnique();
            hierarchyBuilder.Property(mh => mh.Level).HasColumnType("text");
            hierarchyBuilder.Property(mh => mh.ParentId).IsRequired(false);

            hierarchyBuilder
                .HasOne(mh => mh.Person)
                .WithMany(p => p.HierarchyNodes)
                .HasForeignKey(mh => mh.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            hierarchyBuilder
                .HasOne(mh => mh.Parent)
                .WithMany(mh => mh.Subordinates)
                .HasForeignKey(mh => mh.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuarterlyDepartmentSalaryChange>()
                .HasKey(qdsc => qdsc.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.StartDate)
                .HasConversion(dateTimeConverter);

            modelBuilder.Entity<Project>()
                .Property(p => p.EndDate)
                .HasConversion(dateTimeConverter);

            modelBuilder.Entity<TitleHistory>()
                .HasKey(th => th.Id);

            modelBuilder.Entity<TitleHistory>()
                .HasOne(th => th.Person)
                .WithMany(p => p.TitleHistories)
                .HasForeignKey(th => th.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TitleHistory>()
                .Property(th => th.OldTitle)
                .HasColumnType("text");
            modelBuilder.Entity<TitleHistory>()
                .Property(th => th.NewTitle)
                .HasColumnType("text");
            modelBuilder.Entity<TitleHistory>()
                .Property(th => th.DateChanged)
                .HasColumnType("timestamp");
            modelBuilder.Entity<TitleHistory>()
                .Property(th => th.Justification)
                .HasColumnType("text");
                }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<ProjectCandidate> ProjectCandidates { get; set; } = null!;
        public DbSet<SalaryReview> SalaryReviews { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<SalaryReviewHistory> SalaryReviewHistories { get; set; } = null!;
        public DbSet<SalaryReviewRequest> SalaryReviewRequests { get; set; } = null!;
        public DbSet<ManagementHierarchy> ManagementHierarchies { get; set; } = null!;
        public DbSet<QuarterlyDepartmentSalaryChange> QuarterlyDepartmentSalaryChanges { get; set; } = null!;
        public DbSet<TitleHistory> TitleHistories { get; set; } = null!;
    }
}
