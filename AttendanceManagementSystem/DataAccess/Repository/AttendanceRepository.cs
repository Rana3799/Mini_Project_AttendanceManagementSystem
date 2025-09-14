using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.DataAccess.Repository
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Attendance>> GetAttendanceByUserIdAsync(string userId)
        {
            return await _context.Set<Attendance>()
                            .Where(a => a.UserId == userId)
                            .ToListAsync();

        }

        public Task<IEnumerable<Attendance>> GetByUserIdAndMonthAsync(string userId, int year, int month)
        {
            throw new NotImplementedException();
        }

        public async Task<Attendance> GetTodayAttendanceByUserIdAsync(string userId, DateTime dateTime)
        {
            return await _context.Set<Attendance>()
                            .FirstOrDefaultAsync(a => a.UserId == userId && a.AttendanceDate.Date == dateTime.Date);
        }

        public async Task<Attendance>AddAsync(Attendance attendanceRecord)
        {
            await _context.Attendances.AddAsync(attendanceRecord);
            await _context.SaveChangesAsync();
            return attendanceRecord;
        }
    }
}
