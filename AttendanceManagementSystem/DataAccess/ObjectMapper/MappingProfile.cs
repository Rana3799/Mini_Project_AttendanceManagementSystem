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
            CreateMap<UserCreateDto, ApplicationUser>()
     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.userID))
     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
     .ReverseMap();
            CreateMap<AdminRegisterDto, ApplicationUser>().ReverseMap();
            CreateMap <UserUpdateDto,ApplicationUser>().ReverseMap();
        }
    }
}
