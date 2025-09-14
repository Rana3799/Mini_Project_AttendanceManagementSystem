using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] // This authorizes all action methods in this controller
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// Marks a user's attendance for check-in.
        /// </summary>
        /// <param name="attendanceInRequestDto">User ID</param>
        /// <returns>A status code indicating success or failure.</returns>
        [HttpPost("in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkInTime([FromBody] AttendanceInRequestDto attendanceInRequestDto)
        {
            try
            {
                var attendance = await _attendanceService.MarkInTimeAsync(attendanceInRequestDto.UserId);
                return Ok(attendance);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Marks a user's attendance for check-out.
        /// </summary>
        /// <param name="attendanceOutRequestDto">User ID</param>
        /// <returns>A status code indicating success or failure.</returns>
        [HttpPost("out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkOutTime([FromBody] AttendanceOutRequestDto attendanceOutRequestDto)
        {
            try
            {
                var attendance = await _attendanceService.MarkOutTimeAsync(attendanceOutRequestDto.UserId);
                return Ok(attendance);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets a user's monthly attendance report.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="year">The year for the report.</param>
        /// <param name="month">The month for the report.</param>
        /// <returns>A monthly attendance report for the user.</returns>
        [HttpGet("monthly/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MonthlyAttendanceReportDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserMonthlyAttendance(string userId, [FromQuery] int year, [FromQuery] int month)
        {
            var report = await _attendanceService.GetUserMonthlyAttendanceAsync(userId, year, month);
            if (report == null || !report.Any())
            {
                return NotFound();
            }
            return Ok(report);
        }

        /// <summary>
        /// Gets an employee's monthly attendance report (Admin access required).
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="year">The year for the report.</param>
        /// <param name="month">The month for the report.</param>
        /// <returns>A monthly attendance report for the employee.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/monthly/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MonthlyAttendanceReportDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeMonthlyAttendance(string employeeId, [FromQuery] int year, [FromQuery] int month)
        {
            var report = await _attendanceService.GetEmployeeMonthlyAttendanceAsync(employeeId, year, month);
            if (report == null || !report.Any())
            {
                return NotFound();
            }
            return Ok(report);
        }
    }
}
