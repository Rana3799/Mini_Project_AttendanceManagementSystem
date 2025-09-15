namespace AttendanceManagementSystem.DataAccess.Entities.Data
{
    public class Organization
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // string PK
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
