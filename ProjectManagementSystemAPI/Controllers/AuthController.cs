using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using ProjectManagementSystemService;
using System;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthController(AuthService authService, IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email, kullanıcı adı ve şifre boş bırakılamaz");
            }
            try
            {
                await _authService.Register(registerDto);
                return Ok($"{registerDto.Email} kayıt oldu");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email ve şifre boş bırakılamaz");
            }
            try
            {
                return Ok(await _authService.Login(loginDto));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [Authorize]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(PasswordDto passwordDto) {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eski şifre ve yeni şifre boş bırakılamaz");
                }
                return Ok(await _authService.UpdatePassword(passwordDto));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                
            }

        }
        [Authorize]
        [HttpGet("GetUserIdentityId")]
        public async Task<IActionResult> GetUserIdentityId()
        {
            Guid? id = await _authService.GetUserIdentityId();
            if (id == null)
            {
                return NotFound();
            }
            return Ok(id);
        }
        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                _authService.Logout();
                return Ok("Çıkış yapıldı");

            }
            catch (Exception)
            {
                return BadRequest("Çıkış yapılamadı");
                throw;
            }
        }
        [Authorize]
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            try
            {
                return Ok(_authService.GetToken());
            }
            catch (Exception)
            {
                return BadRequest("Token alınamadı");
                throw;
            }
        }
    }
}
