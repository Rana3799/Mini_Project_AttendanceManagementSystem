namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class AdminRegisterDto
    {
        public string UserId { get; set; }   // same as username
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
