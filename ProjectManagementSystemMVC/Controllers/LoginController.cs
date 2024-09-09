using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;
using System;

namespace ProjectManagementSystemMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<User, UserDto, UserUpdateDto> _service;

        public LoginController(AuthService authService, IService<User,UserDto,UserUpdateDto> service)
        {
            _authService = authService;
            _service = service;
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
  
                return Redirect("/UserPage");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("login", exception.Message);
                return View();
            }
        }
    }
}
