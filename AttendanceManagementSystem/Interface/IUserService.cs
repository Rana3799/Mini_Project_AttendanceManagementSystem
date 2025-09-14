using AttendanceManagementSystem.DataAccess.DTO;

namespace AttendanceManagementSystem.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(UserCreateDto userDto);
        Task UpdateAsync(UserUpdateDto userDto);
        Task DeleteAsync(int id);
    }
}

