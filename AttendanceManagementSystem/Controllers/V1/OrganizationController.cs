using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Interface;
using AttendanceManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")] // org mgmt restricted
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _organizationService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) =>
            Ok(await _organizationService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrganizationCreateDto dto)
        {
            var result = await _organizationService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] OrganizationUpdateDto dto) =>
            Ok(await _organizationService.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _organizationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
