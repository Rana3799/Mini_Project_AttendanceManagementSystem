using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Repository;

namespace AttendanceManagementSystem.Interface
{
    public interface IRoleService
    {
    Task<RoleResponseDto> AddRoleAsync();
    Task<UserUpdateDto> UpdateRoleAsync(int id);
    Task DeleteRoleAsync(int id);
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    Task<RoleResponseDto> GetRoleByIdAsync(int id);
    }
}
