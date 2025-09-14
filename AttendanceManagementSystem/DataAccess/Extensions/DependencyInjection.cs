using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Identity;
using AttendanceManagementSystem.DataAccess.IdentityPolicy;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.DataAccess.ObjectMapper;
using AttendanceManagementSystem.DataAccess.Repository;
using AttendanceManagementSystem.Interface;
using AttendanceManagementSystem.Service.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.DataAccess.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register your application/business services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();

            // Register AutoMapper profiles from Application assembly
            services.AddAutoMapper(configAction => configAction.AddProfile<MappingProfile>());

            // Register FluentValidation (if validators are in Application)
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add ASP.NET Core Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                AccountLockPolicy.Configure(options);
                PasswordPolicy.Configure(options);
                SignInAccountPolicy.Configure(options);
                UserDetailPolicy.Configure(options);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Register repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }


    }
}


