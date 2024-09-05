using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IService<Comment, CommentDto, CommentUpdateDto> _service;
        private readonly FileService _fileService;
        public CommentController(IService<Comment,CommentDto,CommentUpdateDto> service, FileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            var result = await _service.GetAll("comments");
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task Add(
                IFormFile? file,
                string text,
                Guid projectId,
                Guid userIdentityId

            )
        {
            CommentDto commentDto =  new CommentDto()
            {
                Id = Guid.NewGuid(),
                Text = text,
                ProjectId = projectId,
                UserIdentityId = userIdentityId
            };
            if (file != null)
            {
                Guid fileUploadId = await _fileService.Upload(file, [".txt"],userIdentityId,null);
                commentDto.FileUploadId = fileUploadId;
                
            }
            await _service.Add(commentDto);
            
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.Get(id));
        }
        [HttpGet("GetCommentsOfProject")]
        public IActionResult GetCommentsOfProject(Guid projectId)
        {
            return Ok(_service.Where(p => p.ProjectId == projectId,"commentsofproject"));
        }
        [HttpGet("GetCommentsOfUser")]
        public IActionResult GetCommentsOfUser(Guid userIdentityId)
        {
            return Ok(_service.Where(p => p.UserIdentityId == userIdentityId,"commentsofusers"));
        }

    }
}
