using AttendanceManagementSystem.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.DataAccess.Entities.Configuration
{
    public static class IdentityModelConfig
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // Users
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users", schema: "identity");
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Roles
            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Roles", schema: "identity");
                entity.HasIndex(r => r.Name).IsUnique();
            });

            // UserRoles
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", schema: "identity");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            // RoleClaims
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: "identity");
            });

            // UserClaims
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", schema: "identity");
            });

            // UserLogins
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", schema: "identity");
            });

            // UserTokens
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", schema: "identity");
            });
        }
    }
}
