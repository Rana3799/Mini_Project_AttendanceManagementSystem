using AttendanceManagementSystem.DataAccess.Extensions;

namespace AttendanceManagementSystem.DataAccess.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetAllWithIncludesAsync();
        Task<IEnumerable<User>> GetUserWithRoleAsync(int id);
    }
}
