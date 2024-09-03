using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IService<Project,ProjectDto,ProjectUpdateDto> _projectService;
        public ProjectController(IService<Project,ProjectDto,ProjectUpdateDto> projectService)
        {
            _projectService = projectService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProjectDto projectDto)
        {
            try
            {
                await _projectService.Add(projectDto);
                return Ok("Proje eklendi");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                throw ;
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _projectService.GetAll());
        }
        [HttpGet("Id")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _projectService.Get(id));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpGet("Name")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                return Ok(await _projectService.Get(x=>x.Name == name));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjectUpdateDto projectUpdateDto)
        {
            try
            {
                await _projectService.Update(projectUpdateDto,projectUpdateDto.Id);
                return Ok("Proje güncellendi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                await _projectService.Remove(id);
                return Ok("Proje silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }




    }
}
