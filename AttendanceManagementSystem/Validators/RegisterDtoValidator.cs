using AttendanceManagementSystem.Controllers.V1;
using AttendanceManagementSystem.DataAccess.DTO;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace AttendanceManagementSystem.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(BeAlphanumeric).WithMessage("UserId must not contain special characters.");

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
               .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }

        private bool BeAlphanumeric(string userId)
        {
            return !string.IsNullOrEmpty(userId) && Regex.IsMatch(userId, @"^[a-zA-Z0-9]+$");
        }
    }
}
