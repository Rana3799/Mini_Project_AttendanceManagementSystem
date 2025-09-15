using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
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

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid request data." });

            // 🔹 Basic validation
            if (string.IsNullOrWhiteSpace(dto.userId))
                return BadRequest(new { message = "UserId is required." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required." });

            // Check if user already exists by username
            var existingUser = await _userManager.FindByNameAsync(dto.userId);
            if (existingUser != null)
                return Conflict(new { message = "User with this UserId already exists." });

            // Check if user already exists by email
            var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmailUser != null)
                return Conflict(new { message = "User with this Email already exists." });

            // Create user
            var user = new ApplicationUser
            {
                UserName = dto.userId,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Ensure User role exists
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var role = new ApplicationRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                };
                await _roleManager.CreateAsync(role);
            }

            // Assign User role
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "User registered successfully" });
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

            var user = await _userManager.FindByEmailAsync(dto.Email);
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("fullName", user.FullName ?? "")
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
