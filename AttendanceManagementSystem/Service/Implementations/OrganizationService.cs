using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Entities.Data;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.Interface;

namespace AttendanceManagementSystem.Service.Implementations
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<OrganizationResponseDto> AddAsync(OrganizationCreateDto dto)
        {
            var org = new Organization
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _organizationRepository.AddAsync(org);

            return new OrganizationResponseDto(org.Id, org.Name, org.Description, org.IsActive, org.CreatedOn);
        }

        public async Task DeleteAsync(string id) =>
            await _organizationRepository.DeleteAsync(id);

        public async Task<IEnumerable<OrganizationResponseDto>> GetAllAsync()
        {
            var list = await _organizationRepository.GetAllAsync();
            return list.Select(org => new OrganizationResponseDto(org.Id, org.Name, org.Description, org.IsActive, org.CreatedOn));
        }

        public async Task<OrganizationResponseDto> GetByIdAsync(string id)
        {
            var org = await _organizationRepository.GetByIdAsync(id);
            if (org == null) throw new KeyNotFoundException("Organization not found");

            return new OrganizationResponseDto(org.Id, org.Name, org.Description, org.IsActive, org.CreatedOn);
        }

        public async Task<OrganizationResponseDto> UpdateAsync(string id, OrganizationUpdateDto dto)
        {
            var org = await _organizationRepository.GetByIdAsync(id);
            if (org == null) throw new KeyNotFoundException("Organization not found");

            org.Name = dto.Name;
            org.Description = dto.Description;
            org.IsActive = dto.IsActive;

            await _organizationRepository.UpdateAsync(org);

            return new OrganizationResponseDto(org.Id, org.Name, org.Description, org.IsActive, org.CreatedOn);
        }
    }
}
