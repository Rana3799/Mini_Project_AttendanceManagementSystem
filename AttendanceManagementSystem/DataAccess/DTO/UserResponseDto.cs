namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
