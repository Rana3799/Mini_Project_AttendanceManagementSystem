namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class RoleCreateDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int HierarchySequence { get; set; }
    }
}
