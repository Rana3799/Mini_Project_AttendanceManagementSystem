using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using System.Globalization;

namespace AttendanceManagementSystem.Interface
{
    public interface IAttendanceService
    {
        Task<Attendance> MarkInTimeAsync(string userId);
        Task<Attendance> MarkOutTimeAsync(string userId);
        Task<IEnumerable<MonthlyAttendanceReportDto>> GetUserMonthlyAttendanceAsync(string userId, int year, int month);
        Task<IEnumerable<MonthlyAttendanceReportDto>> GetEmployeeMonthlyAttendanceAsync(string employeeId, int year, int month);
    }
}
