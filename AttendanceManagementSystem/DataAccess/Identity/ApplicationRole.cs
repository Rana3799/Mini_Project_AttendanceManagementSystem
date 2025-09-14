using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.Identity
{
    public class ApplicationRole : IdentityRole
    {
        // Add custom fields if needed
        public string? Description { get; set; }
    }
}
