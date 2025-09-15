using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Repository;

namespace AttendanceManagementSystem.Interface
{
    public interface IRoleService
    {
        Task<RoleResponseDto> AddRoleAsync(RoleCreateDto dto);
        Task<RoleResponseDto> UpdateRoleAsync(string id, RoleUpdateDto dto);
        Task DeleteRoleAsync(string id);
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
        Task<RoleResponseDto> GetRoleByIdAsync(string id);
    }
}
