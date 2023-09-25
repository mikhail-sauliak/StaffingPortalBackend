using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Person> People { get; set; }
    }
}