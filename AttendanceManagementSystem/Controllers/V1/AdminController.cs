using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Identity;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IValidator<AdminRegisterDto> validator, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new Admin user (SuperAdmin only).
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Invalid request");

                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

                // Check if user already exists by username
                var existingUser = await _userManager.FindByNameAsync(dto.userID);
                if (existingUser != null)
                    return Conflict(new { message = "User with this UserId already exists." });

                // Check if user already exists by email
                var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingEmailUser != null)
                    return Conflict(new { message = "User with this Email already exists." });

                // Map DTO -> ApplicationUser
                ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(dto);
                applicationUser.CreatedBy = User?.Identity?.Name ?? applicationUser.UserName;
                var result = await _userManager.CreateAsync(applicationUser, dto.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

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
                await _userManager.AddToRoleAsync(applicationUser, "Admin");

                return Ok(new { message = "Admin registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
     
    }
}
