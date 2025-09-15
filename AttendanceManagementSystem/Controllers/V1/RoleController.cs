using AttendanceManagementSystem.DataAccess.DTO;
using AttendanceManagementSystem.DataAccess.Extensions;
using AttendanceManagementSystem.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // POST: api/v1/Role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleDto)
        {
            var role = await _roleService.CreateRoleAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        // GET: api/v1/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        // GET: api/v1/Role/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        // PUT: api/v1/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateDto roleDto)
        {
            var role = await _roleService.UpdateRoleAsync(id, roleDto);
            if (role == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/v1/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
