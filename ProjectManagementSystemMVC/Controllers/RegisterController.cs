using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AuthService _authService;
        public RegisterController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("register", "Kullanıcı adı, email ve şifre boş bırakılamaz.");

                return View(registerDto);
            }
            try
            {
                
                await _authService.Register(registerDto);
                return Redirect("/login");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("register", exception.Message);
                return View(registerDto);
            }
        }
    }
}
