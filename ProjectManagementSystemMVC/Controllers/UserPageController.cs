using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Models;

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
            var userIdentity = await _authService.GetUserIdentity(Request);
            return View(userIdentity);
        }
    }
}
