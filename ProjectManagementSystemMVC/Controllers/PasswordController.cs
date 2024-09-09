using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using System;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly AuthService _authService;
        public PasswordController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(PasswordDto passwordDto)
        {
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
