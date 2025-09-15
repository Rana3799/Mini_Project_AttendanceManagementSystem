using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsActive { get; set; } = true;
        public int HierarchySequence { get; set; }

        // Optional additional metadata
        public string? Description { get; set; }
    }
}
