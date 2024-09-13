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
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        public UserPageController(AuthService authService, NotificationService notificationService, IService<Manager, ManagerDto, ManagerUpdateDto> managerService)
        {
            _authService = authService;
            _notificationService = notificationService;
            _managerService = managerService;
        }

        public async Task<IActionResult> Index()
        {
            string? userName = await _authService.GetUserName();
            var id = await _authService.GetUserIdentityId();
            UserPageModel userPageModel = new UserPageModel();
            bool managerExists = await _managerService.Get(x=>x.UserIdentityId == id) != null;
            ViewData["notifications"] = await _notificationService.GetNotifications(id);
            if (managerExists)
            {
                Console.WriteLine(id);
            }
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
