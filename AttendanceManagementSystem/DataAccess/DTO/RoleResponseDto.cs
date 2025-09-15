namespace AttendanceManagementSystem.DataAccess.DTO
{
    public class RoleResponseDto
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int HierarchySequence { get; set; }
    }
}
