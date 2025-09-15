using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        [Authorize] // only logged-in users can access
        [HttpPost("in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkInTime([FromBody] AttendanceInRequestDto attendanceInRequestDto)
        {
            try
            {
                // Get the logged-in user's ID from JWT claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Invalid or missing token." });
                // Allow only Admins to mark for others
                if (userRole != "Admin")
                {
                    // Check if user is trying to mark attendance for someone else
                    if (userId != attendanceInRequestDto.UserId)
                    {
                        return Unauthorized(new
                        {
                            message = "❌ Mark-In failed: Cannot mark attendance for unauthorized user."
                        });
                    }
                }
                var attendance = await _attendanceService.MarkInTimeAsync(attendanceInRequestDto.UserId);

                // Ignore attendanceInRequestDto.UserId and always use the logged-in userId
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

                // Get the logged-in user's ID from JWT claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Invalid or missing token." });
                if (userRole != "Admin")
                {
                    if (userId != attendanceOutRequestDto.UserId) {
                        return Unauthorized(new { message = "Mark-Out failed: Cannot mark attendance for unauthorized user." });}
                }
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
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/monthly/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MonthlyAttendanceReportDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeMonthlyAttendance(
            string employeeId, [FromQuery] int year, [FromQuery] int month)
        {
            var report = await _attendanceService.GetEmployeeMonthlyAttendanceAsync(employeeId, year, month);
            if (report == null || !report.Any())
            {
                return NotFound(new { Message = "No attendance found for this employee." });
            }
            return Ok(report);
        }
    }
}
