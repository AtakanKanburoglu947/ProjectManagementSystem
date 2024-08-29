using Auth;
using AuthService;
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
        private readonly TokenService _tokenService;
        private readonly AppDbContext _appDbContext;
        public AuthController(TokenService tokenService, AppDbContext appDbContext)
        {
            _tokenService = tokenService;
            _appDbContext = appDbContext;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
           
            (string,string) hashedPassword = PasswordHasher.HashPassword(registerDto.Password);
            string passwordHash = hashedPassword.Item1;
            string passwordSalt = hashedPassword.Item2;
            var userIdentity = new UserIdentity()
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserName = registerDto.UserName
            };
            _appDbContext.UserIdentities.Add(userIdentity);
            await _appDbContext.SaveChangesAsync();
            return Ok($"{userIdentity.Email} registered");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            string password = loginDto.Password;
            string email = loginDto.Email;
            DateTime tokenExpiryDate = DateTime.UtcNow.AddHours(1);
            UserIdentity? user = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            bool passwordVerified = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            if (user != null && passwordVerified)
            {
               string token = _tokenService.GenerateToken(user.UserName,user.Email,tokenExpiryDate);
               return Ok(new TokenDto() { Token = token, ExpiryDate = tokenExpiryDate });
            }
            return BadRequest();
        }
        [Authorize]
        [HttpGet("GetData")]
        public IActionResult GetData() {
            string? authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string bearer = "Bearer ";
            if (authorizationHeader != null && authorizationHeader.StartsWith(bearer))
            {
                string? token = authorizationHeader.Substring(bearer.Length).Trim();
                if (_tokenService.ValidateToken(token) == null)
                {
                    return BadRequest();
                }
                

            }
            return Ok("test");
        }
        
    }
}
