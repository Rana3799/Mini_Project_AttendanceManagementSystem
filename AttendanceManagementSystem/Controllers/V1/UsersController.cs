using AttendanceManagementSystem.DataAccess.Constants;
using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Identity;
using AutoMapper;

//using AttendanceManagementSystem.DataAccess.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

//using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UserCreateDto> _validator;
        private readonly IMapper _mapper;
        private readonly IValidator<AdminRegisterDto> _adminvalidator;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IValidator<UserCreateDto> validator,
            IValidator<AdminRegisterDto> adminvalidator,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _validator = validator;
            _adminvalidator = adminvalidator;
            _mapper = mapper;
        }


        /// <summary>
        /// Register a new Admin user (SuperAdmin only).
        /// </summary>
        [HttpPost("Admin-register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Invalid request");

                var validationResult = await _adminvalidator.ValidateAsync(dto);
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


        // POST: api/users/register
        [HttpPost("User-register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { message = "Invalid request data." });

                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

                if (string.IsNullOrWhiteSpace(dto.userID))
                    return BadRequest(new { message = "UserId is required." });

                if (string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new { message = "Email is required." });

                if (string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest(new { message = "Password is required." });

                var existingUser = await _userManager.FindByNameAsync(dto.userID);
                if (existingUser != null)
                    return Conflict(new { message = "User with this UserId already exists." });

                var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingEmailUser != null)
                    return Conflict(new { message = "User with this Email already exists." });

                // 🔹 Ensure mapping sets UserName and Email
                ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(dto);
                // Set audit info before saving
                applicationUser.CreatedBy = User?.Identity?.Name ?? applicationUser.UserName;
                //applicationUser.CreatedOn = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(applicationUser, dto.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                if (!await _roleManager.RoleExistsAsync(RoleConstants.User))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = RoleConstants.User,
                        NormalizedName = RoleConstants.User.ToUpper()
                    });
                }

                await _userManager.AddToRoleAsync(applicationUser, RoleConstants.User);

                return Ok(new { message = "User registered successfully" });
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



        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, [FromServices] IValidator<LoginDto> validator)
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Change from FindByEmailAsync -> FindByNameAsync
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var token = await GenerateJwtToken(user);
            return Ok(new { token });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
               // new Claim("fullName", user.FullName ?? "")
            };

            claims.AddRange(userClaims);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
