using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
    public class AppdbContext:DbContext
    {
        public AppdbContext(DbContextOptions<AppdbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AttendancePunch> AttendancePunches { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AttendancePunch>()
                .HasKey(ap => ap.AttendanceId);
            modelBuilder.Entity<AttendancePunch>()
                .HasOne(ap => ap.Employee)
                .WithMany(e=>e.AttendancePunches)
                .HasForeignKey(ap => ap.EmployeeId);
            modelBuilder.Entity<AttendancePunch>()
               .Property(ap => ap.Status)
               .HasConversion<int>();
        }
    }
}
