using Kriya2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kriya2.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<College> Colleges { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<StudentLogin> StudentLogins { get; set; }
        public DbSet<CollegeLogin> collegeLogins { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraint for Student USN
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.USN)
                .IsUnique();

            // Unique constraint for College Name
            modelBuilder.Entity<College>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Unique constraint for Event Title
            modelBuilder.Entity<Event>()
                .HasIndex(e => e.Title)
                .IsUnique();
        }
        public DbSet<Kriya2.Models.Entities.CollegeLogin> CollegeLogin { get; set; } = default!;
    }
}
