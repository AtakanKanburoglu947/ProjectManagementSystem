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
        private readonly FileService _fileService;
        public ProjectController(IService<Project,ProjectDto,ProjectUpdateDto> projectService, FileService fileService)
        {
            _projectService = projectService;
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(
                 IFormFile? file,
                 string Name,
                 string Description,
                 string Version,
                 string Status,
                 int ManagerId
            )
        {
            try
            {
                ProjectDto projectDto = new ProjectDto()
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Description = Description,
                    Version = Version,
                    Status = Status,
                    ManagerId = ManagerId
                };
                if (file != null)
                {
                    Guid fileUploadId = await _fileService.Upload(file, [".txt"],null,projectDto.ManagerId);
                    projectDto.FileUploadId = fileUploadId;
                }
                await _projectService.Add(projectDto);
                return Ok("Proje eklendi");
            }
            catch (Exception)
            {
                return BadRequest("Proje eklenemedi");
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
