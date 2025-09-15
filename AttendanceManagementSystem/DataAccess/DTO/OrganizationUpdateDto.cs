namespace AttendanceManagementSystem.DataAccess.DTO
{
    public record OrganizationUpdateDto(string Name, string? Description, bool IsActive);
}
