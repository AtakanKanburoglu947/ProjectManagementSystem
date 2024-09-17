using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;
        private readonly CacheService _cacheService;
        private readonly FileService _fileService;
        public AccountController(AuthService authService, NotificationService notificationService, CacheService cacheService,FileService fileService)
        {
            _authService = authService;
            _notificationService = notificationService;
            _cacheService = cacheService;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);
            UserIdentity cache = await _cacheService.Get("account",userIdentityId,TimeSpan.FromHours(1),TimeSpan.FromMinutes(3), 
                async () => await _authService.GetUserById(await _authService.GetUserIdentityId()));
            
            AccountPageModel accountPageModel = new AccountPageModel()
            {
                Email = cache.Email,
                UserName = cache.UserName,

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
