using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IValidator<AdminRegisterDto> _validator;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IValidator<AdminRegisterDto> validator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
        }

        /// <summary>
        /// Register a new Admin user (SuperAdmin only).
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (dto == null)
                return BadRequest("Invalid request");

            // Check if user already exists by username
            var existingUser = await _userManager.FindByNameAsync(dto.UserId);
            if (existingUser != null)
                return Conflict(new { message = "User with this UserId already exists." });

            // Check if user already exists by email
            var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmailUser != null)
                return Conflict(new { message = "User with this Email already exists." });

            var user = new ApplicationUser
            {
                UserName = dto.UserId,
                Email = dto.Email,
               // FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Ensure Admin role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new ApplicationRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                await _roleManager.CreateAsync(role);
            }

            // Assign Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { message = "Admin registered successfully" });
        }

    }
}
