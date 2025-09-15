using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Identity;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.Interface;

namespace AttendanceManagementSystem.Service.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponseDto> AddRoleAsync(RoleCreateDto dto)
        {
            var role = new ApplicationRole
            {
                Name = dto.RoleName,
                NormalizedName = dto.RoleName.ToUpper(),
                Description = dto.Description,
                IsActive = dto.IsActive,
                HierarchySequence = dto.HierarchySequence
            };

            var createdRole = await _roleRepository.AddAsync(role);
            return MapToResponse(createdRole);
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleUpdateDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Role not found");

            role.Description = dto.Description;
            role.IsActive = dto.IsActive;
            role.HierarchySequence = dto.HierarchySequence;

            await _roleRepository.UpdateAsync(role);

            return MapToResponse(role);
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _roleRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Role not found");

            await _roleRepository.DeleteAsync(role);
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(MapToResponse);
        }

        public async Task<RoleResponseDto> GetRoleByIdAsync(string id)
        {
            var role = await _roleRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Role not found");

            return MapToResponse(role);
        }

        private static RoleResponseDto MapToResponse(ApplicationRole role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                RoleName = role.Name!,
                Description = role.Description,
                IsActive = role.IsActive,
                HierarchySequence = role.HierarchySequence
            };
        }
    }
}
