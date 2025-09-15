namespace AttendanceManagementSystem.DataAccess.DTO
{
    public record OrganizationResponseDto(string Id, string Name, string? Description, bool IsActive, DateTime CreatedOn);
}
