using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementSystem.DataAccess.Extensions
{
    public class User
    {
        public int Id { get; set; }
        [Key]
        public int UserId { get; set; } 
        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public char Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? PasswordHash { get; set; }   
        public bool IsAccountLocked { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
