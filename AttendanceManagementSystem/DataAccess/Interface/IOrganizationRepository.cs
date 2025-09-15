using AttendanceManagementSystem.DataAccess.Entities.Data;

namespace AttendanceManagementSystem.DataAccess.Interface
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetByIdAsync(string id);
        Task<IEnumerable<Organization>> GetAllAsync();
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(string id);
    }
}
