namespace AttendanceManagementSystem.DataAccess.Extensions
{
    public class Attendance
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime AttendanceDate { get; set; } = DateTime.UtcNow;
        public string? Type { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
