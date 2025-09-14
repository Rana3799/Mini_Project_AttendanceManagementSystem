using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.IdentityPolicy
{
    public static class PasswordPolicy
    {
        public static void Configure(IdentityOptions options)
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        }
    }
}
