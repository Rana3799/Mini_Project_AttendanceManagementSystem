using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Identity;
using FluentValidation;

namespace AttendanceManagementSystem.DataAccess.Validators
{
    public class UserCreateDtoValidator: AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Gender).NotEmpty().Must(g => g == 'M' || g == 'F');
        }
    }
}
