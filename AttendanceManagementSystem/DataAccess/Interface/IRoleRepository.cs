using AttendanceManagementSystem.DataAccess.Identity;

namespace AttendanceManagementSystem.DataAccess.Interface
{
    public interface IRoleRepository
    {
        Task<ApplicationRole?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task<ApplicationRole> AddAsync(ApplicationRole role);
        Task UpdateAsync(ApplicationRole role);
        Task DeleteAsync(ApplicationRole role);
    }
}
