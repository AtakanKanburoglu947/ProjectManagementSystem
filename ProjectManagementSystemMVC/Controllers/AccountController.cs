using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly FileService _fileService;
        public AccountController(AuthService authService,FileService fileService)
        {
            _authService = authService;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            var files =  await _fileService.GetFilesOfUser(userIdentityId);
            AccountPageModel accountPageModel = new AccountPageModel()
            {
                Email = userIdentity.Email,
                UserName = userIdentity.UserName,
                Files = files
            };
          
            return View(accountPageModel);
        }
        public async Task<IActionResult> Download(Guid id)
        {
            FileUpload? file = await _fileService.GetFile(id);
            return File(file.Data, "application/octet-stream", fileDownloadName: file.Name);
        }
    }
}
