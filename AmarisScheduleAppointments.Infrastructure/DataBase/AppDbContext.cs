using AmarisScheduleAppointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmarisScheduleAppointments.Infrastructure.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Branch)
                .WithMany(b => b.Shifts)
                .HasForeignKey(s => s.BranchId);
        }
    }
}
