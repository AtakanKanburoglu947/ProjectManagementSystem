using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;
        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        public async Task Upload(IFormFile file, Guid userIdentityId, Guid? projectId, Guid? commentId)
        {

        }
    
    }
}
