using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService) {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleDto roleDto)
        {
            try
            {
                await _roleService.Add(roleDto);
                return Ok("Rol eklendi");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                
            }
        }
    }
}
