using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;

namespace AttendanceManagementSystem.Interface
{
    public interface IOrganizationService
    {
        Task<OrganizationResponseDto> GetByIdAsync(string id);
        Task<IEnumerable<OrganizationResponseDto>> GetAllAsync();
        Task<OrganizationResponseDto> AddAsync(OrganizationCreateDto dto);
        Task<OrganizationResponseDto> UpdateAsync(string id, OrganizationUpdateDto dto);
        Task DeleteAsync(string id);
    }
}
