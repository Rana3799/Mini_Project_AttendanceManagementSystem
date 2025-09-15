using AttendanceManagementSystem.Controllers.V1;
using AttendanceManagementSystem.DataAccess.DTO;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace AttendanceManagementSystem.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

}
