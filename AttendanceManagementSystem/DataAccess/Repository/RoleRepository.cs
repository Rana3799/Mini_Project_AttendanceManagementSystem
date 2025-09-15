using AttendanceManagementSystem.DataAccess.Identity;
using AttendanceManagementSystem.DataAccess.Interface;
using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole?> GetByIdAsync(string id)
            => await _roleManager.FindByIdAsync(id);

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
            => _roleManager.Roles.ToList();

        public async Task<ApplicationRole> AddAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return role;
        }

        public async Task UpdateAsync(ApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteAsync(ApplicationRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
