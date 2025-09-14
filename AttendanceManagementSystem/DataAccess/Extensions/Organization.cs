namespace AttendanceManagementSystem.DataAccess.Extensions
{
    public class Organization
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string? Location { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
