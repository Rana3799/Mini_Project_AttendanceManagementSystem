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
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required.")
                .WithMessage("Invalid User Name.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

}
