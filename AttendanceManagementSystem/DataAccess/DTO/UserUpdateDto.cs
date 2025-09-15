using System.Text.Json.Serialization;

namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class UserUpdateDto : UserCreateDto
    {
        [JsonPropertyName("updateUserId")]
        public string UpdateUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; }
        public char Gender { get; set; }
        public DateOnly DOB { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
    }
}
