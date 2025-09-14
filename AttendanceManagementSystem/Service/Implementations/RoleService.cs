using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.Interface;
using AutoMapper;

namespace AttendanceManagementSystem.Service.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        public Task<RoleResponseDto> AddRoleAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserUpdateDto> UpdateRoleAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<RoleResponseDto>> IRoleService.GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        Task<RoleResponseDto> IRoleService.GetRoleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
