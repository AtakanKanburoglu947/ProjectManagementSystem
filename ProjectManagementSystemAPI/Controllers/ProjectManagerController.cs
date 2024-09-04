using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagerController : ControllerBase
    {
        private readonly IService<ProjectManager,ProjectManager,ProjectManager> _projectManagerService;
        public ProjectManagerController(IService<ProjectManager, ProjectManager, ProjectManager> projectManagerService)
        {
            _projectManagerService = projectManagerService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _projectManagerService.GetAll());
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid projectId, int managerId)
        {
            var project = await _projectManagerService.Get(x=>x.ProjectId == projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        [HttpPost]
        public async Task Add(ProjectManager projectManager)
        {
            await _projectManagerService.Add(projectManager);
        }
        [HttpDelete]
        public async Task Remove(Guid projectId, int managerId)
        {
            await _projectManagerService.Remove(projectId,managerId);
        }
        [HttpGet("Where")]
        public  IActionResult Where(Guid projectId)
        {
            var result = _projectManagerService.Where(x => x.ProjectId == projectId);
            if (result != null)
            {
                return Ok(result);
            }
            else { return NotFound(); }
        }
    }
}
