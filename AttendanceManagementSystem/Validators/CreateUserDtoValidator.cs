using AttendanceManagementSystem.DataAccess.DTO;
using FluentValidation;
using FluentValidation.Validators;

namespace AttendanceManagementSystem.Validators
{
    public class CreateUserDtoValidator: AbstractValidator<RegisterDto>
    {
        public CreateUserDtoValidator() {
            RuleFor(x => x.userId)
                   .NotEmpty().WithMessage("UserId is required.")
                   .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("UserId can contain only letters, numbers, and underscores.")
                   .MinimumLength(3).WithMessage("UserId must be at least 3 characters long.");

            // Email: required, valid format, ASP.NET compatible
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email format.");

            // FullName: required, letters and spaces only
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Full name can contain only letters and spaces.");

            // Password: required, minimum 6 chars
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
    
}
