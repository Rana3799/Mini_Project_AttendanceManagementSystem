using AttendanceManagementSystem.DataAccess.Constants;
using AttendanceManagementSystem.DataAccess.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceManagementSystem.DataAccess.Entities.Configuration
{
    public class GroupRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = RoleConstants.AdminRoleId,
                    Name = RoleConstants.Admin,
                    NormalizedName = RoleConstants.Admin.ToUpper(),
                    IsActive = true,
                    HierarchySequence = 1,
                    Description = "Administrator role with full permissions"
                },
                new ApplicationRole
                {
                    Id = RoleConstants.UserRoleId,
                    Name = RoleConstants.User,
                    NormalizedName = RoleConstants.User.ToUpper(),
                    IsActive = true,
                    HierarchySequence = 2,
                    Description = "Standard application user"
                }
            );

        }
    }
}
