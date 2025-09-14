using AttendanceManagementSystem.DataAccess.DTO;
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

        public async Task<IEnumerable<Attendance>> GetByUserIdAndMonthAsync(string userId, int year, int month)
        {
            return await _context.Attendances
                .Where(a => a.UserId == userId &&
                            a.AttendanceDate.Year == year &&
                            a.AttendanceDate.Month == month)
                .ToListAsync();
        }


        public async Task<Attendance> AddAsync(Attendance attendanceRecord)
        {
            await _context.Attendances.AddAsync(attendanceRecord);
            await _context.SaveChangesAsync();
            return attendanceRecord;
        }

        public async Task<IEnumerable<MonthlyAttendanceReportDto>> GetUserMonthlyAttendanceAsync(string userId, int year, int month)
        {
                return await _context.Attendances
            .Where(a => a.UserId == userId
                     && a.AttendanceDate.Year == year
                     && a.AttendanceDate.Month == month)
            .Select(a => new MonthlyAttendanceReportDto
            {
                UserId = a.UserId,
                Date = a.AttendanceDate,
                CheckInTime = a.Type == "IN" ? a.AttendanceDate : null,
                CheckOutTime = a.Type == "OUT" ? a.AttendanceDate : null
            })
            .ToListAsync();
        }

        public async Task<IEnumerable<MonthlyAttendanceReportDto>> GetEmployeeMonthlyAttendanceAsync(string userId, int year, int month)
        {
            return await _context.Attendances
                .Where(a => a.UserId == userId
                         && a.AttendanceDate.Year == year
                         && a.AttendanceDate.Month == month)
                .Select(a => new MonthlyAttendanceReportDto
                {
                    UserId = a.UserId,
                    Date = a.AttendanceDate,
                    CheckInTime = a.Type == "IN" ? a.AttendanceDate : null,
                    CheckOutTime = a.Type == "OUT" ? a.AttendanceDate : null
                })
                .ToListAsync();
        }

        public async Task<Attendance> GetTodayAttendanceByUserIdAsync(string userId, DateTime dateTime)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));

            // Fetch today's attendance record for the given user
            return await _context.Attendances
                .FirstOrDefaultAsync(a => a.UserId == userId
                                       && a.AttendanceDate.Date == dateTime.Date);
        }


    }
}
