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
    public class UserPageController : Controller
    {
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;
        public UserPageController(AuthService authService, NotificationService notificationService)
        {
            _authService = authService;
            _notificationService = notificationService;

        }

        public async Task<IActionResult> Index()
        {
            string? userName = await _authService.GetUserName();
            var id = await _authService.GetUserIdentityId();
            UserPageModel userPageModel = new UserPageModel();
            ViewData["notifications"] = await _notificationService.GetNotifications(id);
            userPageModel.UserName = userName;
            return View(userPageModel);
        }
        public IActionResult Logout()
        {
            _authService.Logout();
            return Redirect("/");
        }
    }
}
