using Microsoft.AspNetCore.Identity;

namespace AttendanceManagementSystem.DataAccess.IdentityPolicy
{
    public static class AccountLockPolicy
    {
        public static void Configure(IdentityOptions options)
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        }
    }
}
