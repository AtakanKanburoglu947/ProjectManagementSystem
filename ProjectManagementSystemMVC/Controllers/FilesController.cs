using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly AuthService _authService;
        private readonly FileService _fileService;
        public FilesController(AuthService authService, FileService fileService)
        {
            _authService = authService;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewData["id"] = id;
            Guid userIdentityId = await _authService.GetUserIdentityId();
            List<FileUpload> files = await _fileService.Filter(id,x=>x.UserIdentityId == userIdentityId);
            int count = _fileService.Count(x=>x.UserIdentityId == userIdentityId);
            PaginationModel<FileUpload,NoData> paginationModel = Pagination<FileUpload,NoData>.Model(id,userIdentityId,null,files,count);
            return View(paginationModel);
        }
    }
}
