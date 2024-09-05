using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;
using System.Linq.Expressions;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectUserController : ControllerBase
    {
        private readonly IService<ProjectUser,ProjectUser,ProjectUser> _service;
        public ProjectUserController(IService<ProjectUser,ProjectUser,ProjectUser> service)
        {
            _service = service;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
        
            return Ok(await _service.GetAll("projectusers"));
        }
        [HttpPost]
        public async Task Add(ProjectUser projectUser)
        {
            await _service.Add(projectUser);
        }
        [HttpGet] 
        public async Task<IActionResult> Get(Guid projectId, int userId)
        {
            var result = await _service.Get(projectId, userId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("Where")]
        public IActionResult Where(Guid projectId)
        {
            var result = _service.Where(x=>x.ProjectId == projectId,"usersofproject");
            if (result != null) { return Ok(result); }
            return BadRequest();
        }
        [HttpDelete]
        public async Task Remove(Guid projectId, int userId)
        {
            await _service.Remove(userId,projectId);
        }
    }
}
