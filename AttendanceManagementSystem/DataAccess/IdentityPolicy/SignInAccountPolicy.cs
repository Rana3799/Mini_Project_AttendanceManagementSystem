using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.IdentityPolicy
{
    public static class SignInAccountPolicy
    {
        public static void Configure(IdentityOptions options)
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // Lockout settings
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.AllowedForNewUsers = true;
        }
    }
}
