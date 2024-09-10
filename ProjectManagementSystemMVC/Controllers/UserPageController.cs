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


        public UserPageController(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            string? userName = await _authService.GetUserName();
            UserPageModel userPageModel = new UserPageModel();
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
