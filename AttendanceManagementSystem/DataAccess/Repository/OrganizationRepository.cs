using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;

namespace AttendanceManagementSystem.DataAccess.Repository
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
