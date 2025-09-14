using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Service.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<Attendance> MarkInTimeAsync(string userId)
        {
            var now = DateTime.Now.TimeOfDay;
            var inTimeStart = new TimeSpan(14, 0, 0); // 11:00 AM
            var inTimeEnd = new TimeSpan(15, 30, 0);  // 11:30 AM

            if (now < inTimeStart || now > inTimeEnd)
            {
                // Handle case where time is outside the allowed slot
                throw new InvalidOperationException("Check-in is only allowed between 11:00 AM and 11:30 AM.");
            }

            var existingAttendance = await _attendanceRepository.GetTodayAttendanceByUserIdAsync(userId, DateTime.Today);
            if (existingAttendance != null && existingAttendance.Type == "IN")
            {
                throw new InvalidOperationException("You have already checked in today.");
            }

            var attendanceRecord = new Attendance
            {
                UserId = userId,
                AttendanceDate = DateTime.UtcNow,
                Type = "IN"
            };

            return await _attendanceRepository.AddAsync(attendanceRecord);
        }

        public async Task<Attendance> MarkOutTimeAsync(string userId)
        {
            var now = DateTime.Now.TimeOfDay;
            var outTimeStart = new TimeSpan(8, 0, 0); // 8:00 AM
            var outTimeEnd = new TimeSpan(8, 30, 0);  // 8:30 AM

            if (now < outTimeStart || now > outTimeEnd)
            {
                // Handle case where time is outside the allowed slot
                throw new InvalidOperationException("Check-out is only allowed between 8:00 AM and 8:30 AM.");
            }

            var existingAttendance = await _attendanceRepository.GetTodayAttendanceByUserIdAsync(userId, DateTime.Today);
            if (existingAttendance == null || existingAttendance.Type == "OUT")
            {
                throw new InvalidOperationException("You can only check out after checking in.");
            }

            var attendanceRecord = new Attendance
            {
                UserId = userId,
                AttendanceDate = DateTime.UtcNow,
                Type = "OUT"
            };

            return await _attendanceRepository.AddAsync(attendanceRecord);
        }

        public async Task<IEnumerable<MonthlyAttendanceReportDto>> GetUserMonthlyAttendanceAsync(string userId, int year, int month)
        {
            // Business logic to retrieve and format user's monthly attendance
            var allRecords = await _attendanceRepository.GetByUserIdAndMonthAsync(userId, year, month);

            // Group records by date to get a single entry per day
            var report = allRecords.GroupBy(a => a.AttendanceDate.Date)
                                   .Select(g => new MonthlyAttendanceReportDto
                                   {
                                       //UserId = g.Fir,
                                       Date = g.Key,
                                       CheckInTime = g.FirstOrDefault(a => a.Type == "IN")?.AttendanceDate,
                                       CheckOutTime = g.FirstOrDefault(a => a.Type == "OUT")?.AttendanceDate
                                   });
            return report;
        }

        // This method can call the same logic as the user's method.
        public async Task<IEnumerable<MonthlyAttendanceReportDto>> GetEmployeeMonthlyAttendanceAsync(string employeeId, int year, int month)
        {
            return await GetUserMonthlyAttendanceAsync(employeeId, year, month);
        }
    }
}