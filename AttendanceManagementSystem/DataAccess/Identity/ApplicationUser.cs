using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementSystem.DataAccess.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields if needed
        public string userID { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public DateOnly DOB { get; set; }
        public bool IsAccountLocked { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int RoleName { get; set; }
        public int OrganizationName { get; set; }
        public string CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
