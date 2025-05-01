using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core;

namespace TimeTracking.Infrastructure
{
    public class TimeTrackingContext : DbContext
    {
        public TimeTrackingContext(DbContextOptions<TimeTrackingContext> options) 
            : base(options) 
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>()
                .HasIndex(a => new { a.EmployeeId, a.Date });

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Employee>()
               .Property(e => e.Sex)
               .HasConversion<string>()
               .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Activities)
                .HasForeignKey(a => a.EmployeeId);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.ProjectId);

            modelBuilder.Entity<Activity>()
                .Property(a => a.Role)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<Activity>()
                .Property(a => a.ActivityType)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
