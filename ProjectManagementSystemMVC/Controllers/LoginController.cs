using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using System;

namespace ProjectManagementSystemMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        public LoginController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("login", "Email ve şifre boş bırakılamaz.");

                return View(loginDto);
            }
            try
            {
                await _authService.Login(loginDto);
                return Redirect("/");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("login", exception.Message);
                return View(loginDto);
            }
        }
    }
}
