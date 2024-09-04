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
        public CommentController(IService<Comment,CommentDto,CommentUpdateDto> service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var result = await _service.GetAll("comments");
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task Add(CommentDto commentDto)
        {
            await _service.Add(commentDto);
            
        }
    }
}
