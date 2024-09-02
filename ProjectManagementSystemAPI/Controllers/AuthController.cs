using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
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
                return Ok(await _authService.UpdatePassword(Request,passwordDto));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                
            }

        }

    }
}
