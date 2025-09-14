namespace AttendanceManagementSystem.DataAccess.Extensions
{
    public class Role
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; } = true;
        public int HierarchySequence { get; set; }
    }
}
