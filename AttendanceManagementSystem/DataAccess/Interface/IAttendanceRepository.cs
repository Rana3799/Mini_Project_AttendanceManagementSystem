using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;

namespace AttendanceManagementSystem.DataAccess.Interface
{
    public interface IAttendanceRepository
    {
            Task<IEnumerable<Attendance>> GetAttendanceByUserIdAsync(string userId);
            Task<Attendance> GetTodayAttendanceByUserIdAsync(string userId, DateTime dateTime);
            // You'll also need a method to add a new attendance record
            Task<Attendance> AddAsync(Attendance attendanceRecord);
        Task<IEnumerable<Attendance>> GetByUserIdAndMonthAsync(string userId, int year, int month);
    }
}

