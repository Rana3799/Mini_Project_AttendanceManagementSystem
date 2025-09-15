namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class RoleUpdateDto
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int HierarchySequence { get; set; }
    }
}
