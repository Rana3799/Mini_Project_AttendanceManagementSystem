using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.DataAccess.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Organization> _dbSet;

        public OrganizationRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Organization>();
        }

        public async Task<Organization?> GetByIdAsync(string id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<Organization>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task AddAsync(Organization organization)
        {
            await _dbSet.AddAsync(organization);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Organization organization)
        {
            _dbSet.Update(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var org = await _dbSet.FindAsync(id);
            if (org != null)
            {
                _dbSet.Remove(org);
                await _context.SaveChangesAsync();
            }
        }
    }
}
