using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;

namespace AttendanceManagementSystem.DataAccess.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }

        public Task<IEnumerable<User>> GetAllWithIncludesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUserWithRoleAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
