namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class UserCreateDto
    {
        public string userID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; }
        public char Gender { get; set; }
        public DateOnly DOB { get; set; }
        public string Password { get; set; } = null!;
    }
}
