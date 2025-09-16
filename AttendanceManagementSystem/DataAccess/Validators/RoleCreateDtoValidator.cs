using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using FluentValidation;

namespace AttendanceManagementSystem.DataAccess.Validators
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateDtoValidator()
        {
            // Rule for RoleName
            // It must not be empty and its length must be between 1 and 200 characters.
            // This validates the "Can Not be Empty" and "VARCHAR(200)" requirements.
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name cannot be empty.")
                .MaximumLength(200).WithMessage("Role name cannot exceed 200 characters.");

            // Rule for HierarchySequence
            // The hierarchy sequence must be a non-negative number.
            RuleFor(x => x.HierarchySequence)
                .GreaterThanOrEqualTo(0).WithMessage("Hierarchy sequence must be a non-negative number.");

            // Rule for IsActive (optional but good practice)
            // You can add a rule here if you have specific constraints.
            // For now, it's a simple boolean so no special validation is needed.
        }
    }
}
