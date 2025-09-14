using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
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
        }
    }
}
