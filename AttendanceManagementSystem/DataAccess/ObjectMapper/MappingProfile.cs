using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Identity;
using AutoMapper;

namespace AttendanceManagementSystem.DataAccess.ObjectMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Example: Domain User -> DTO
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<UserUpdateDto, User>(); 
            CreateMap<UserCreateDto, ApplicationUser>().ReverseMap();
            CreateMap <UserUpdateDto,ApplicationUser>().ReverseMap();
        }
    }
}
