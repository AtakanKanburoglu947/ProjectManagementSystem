using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemService;
using System;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;
        public PasswordController(AuthService authService, NotificationService notificationService)
        {
            _authService = authService;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            Guid id = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(id);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(PasswordDto passwordDto)

        {
            Guid id = await _authService.GetUserIdentityId();

            ViewData["notifications"] = await _notificationService.GetNotifications(id);
            ViewData["Message"] = string.Empty;
            if (passwordDto.NewPassword == null || passwordDto.OldPassword == null)
            {
                ModelState.AddModelError("password", "Eski şifre ve yeni şifre boş bırakılamaz");

            }
            try
            {
                await _authService.UpdatePassword(passwordDto);
                ViewData["Message"] = "Şifre güncellendi";
                return View();
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("password",exception.Message);
                return View();
            }

        }
    }
}
