using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using AttendanceManagementSystem.MailConfig;


namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        // add them to constructor
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            IMemoryCache memoryCache,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
            _env = env;
        }


        // Register a normal user
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null) return BadRequest("Invalid request.");

            var existingUser = await _userManager.FindByNameAsync(dto.userId);
            if (existingUser != null) return Conflict("User with this UserId already exists.");

            var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmailUser != null) return Conflict("User with this Email already exists.");

            var user = new ApplicationUser { UserName = dto.userId, Email = dto.Email, FullName = dto.FullName };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            // Ensure User role exists
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new ApplicationRole { Name = "User", NormalizedName = "USER" });

            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "User registered successfully" });
        }

        // Register an Admin (SuperAdmin only)
        [HttpPost("register-admin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.UserId);
            if (existingUser != null) return Conflict("User with this UserId already exists.");

            var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingEmailUser != null) return Conflict("User with this Email already exists.");

            var user = new ApplicationUser { UserName = dto.UserId, Email = dto.Email, FullName = dto.FullName };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN" });

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { message = "Admin registered successfully" });
        }

        // Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, [FromServices] IValidator<LoginDto> validator)
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid credentials");

            var token = await GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Reset Password
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                // For security, don't reveal if the email exists or not.
                return BadRequest("Invalid request.");
            }

            // Decode the token (for reset)
            string decodedToken;
            try
            {
                var tokenBytes = WebEncoders.Base64UrlDecode(dto.Token);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch
            {
                return BadRequest("Invalid token.");
            }

            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new { message = "Unable to reset password.", errors });
            }

            // Send email to user confirming password reset
            var subject = "Password Reset Successful";
            var body = $"<p>Hello {user.FullName ?? user.UserName},</p>" +
                       "<p>the password has been successfully reset. If you did not request this change, please contact support.</p>";

            await _emailSender.SendEmailAsync(user.Email, subject, body);

            return Ok(new { message = "Password has been reset successfully." });
        }

        // Confirm Email
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Email confirmed successfully. You can now log in." });
        }

        // Generate JWT
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

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // Validate DTO via FluentValidation if you want automatic behavior; or do manual check:
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            // Rate limit per email (e.g., 5 attempts per 1 hour)
            var key = $"forgotpwd_{dto.Email.ToLower()}";
            var attempts = _memoryCache.Get<int>(key);
            if (attempts >= 5)
                return StatusCode(429, "Too many password reset requests. Please try later.");

            _memoryCache.Set(key, attempts + 1, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            var user = await _userManager.FindByEmailAsync(dto.Email);
            // Don't reveal whether the user exists; return generic response.
            if (user == null)
            {
                // Optionally log a warning, but return generic message
                return Ok(new { message = "Account does not exist." });
            }

            // Optionally check user email confirmed: up to the policy
            // if (!await _userManager.IsEmailConfirmedAsync(user))
            //     return Ok(new { message = "If an account with that email exists, a password reset link has been sent." });

            // Generate token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);

            // Build reset URL for front-end (configure in appsettings)
            var frontendBase = _configuration["ApplicationSetup:FrontendUrl"]?.TrimEnd('/');
            if (string.IsNullOrEmpty(frontendBase))
            {
                // fallback to API confirm endpoint if FrontendUrl isn't set
                frontendBase = _configuration["ApplicationSetup:ApiBaseUrl"]?.TrimEnd('/');
            }

            var resetUrl = $"{frontendBase}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={encodedToken}";

            // Compose email
            var subject = "Reset the password";
            var body = EmailTemplates.GetResetPasswordHtml(user.FullName ?? user.UserName, resetUrl);

            // Send email (in production you may want to schedule/queue it)
            await _emailSender.SendEmailAsync(user.Email, subject, body);

            // For Development: optionally return token in response to ease testing (do not enable in production)
            if (_env.IsDevelopment())
            {
                return Ok(new { message = "Password reset link sent (development).", token = encodedToken });
            }

            return Ok(new { message = "If an account with that email exists, a password reset link has been sent." });
        }


    }
}
