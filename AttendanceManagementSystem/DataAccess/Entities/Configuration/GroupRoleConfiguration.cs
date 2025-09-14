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
                    Id = RoleConstants.AdminRoleId, // static GUID to escape migration regeneration issues
                    Name = RoleConstants.Admin,
                    NormalizedName = RoleConstants.Admin.ToUpper()
                },
                new ApplicationRole
                {
                    Id = RoleConstants.UserRoleId, // static GUID to escape migration regeneration issues
                    Name = RoleConstants.User,
                    NormalizedName = RoleConstants.User.ToUpper()
                }
            );
        }
    }
}
