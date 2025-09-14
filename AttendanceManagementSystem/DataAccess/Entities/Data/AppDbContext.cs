using AttendanceManagementSystem.DataAccess.Entities.Configuration;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.DataAccess.Entities.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Keeping domain entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity table mapping
            IdentityModelConfig.Configure(modelBuilder);

            // Role seed
            modelBuilder.ApplyConfiguration(new GroupRoleConfiguration());
        }
    }
}
