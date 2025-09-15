namespace AttendanceManagementSystem.DataAccess.DTO
{
    public record ResetPasswordDto(string Email, string Token, string? NewPassword);
}
