using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class AuthService 
    {
        private AppDbContext _appDbContext;
        private TokenService _tokenService;
        public AuthService(AppDbContext appDbContext, TokenService tokenService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            UserIdentity? user = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return user != null;
        }
        public bool PasswordVerified(string password, UserIdentity user)
        {
            return PasswordService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }
        public async Task Register(RegisterDto registerDto)
        {
            (string, string) hashedPassword = PasswordService.HashPassword(registerDto.Password);
            string passwordHash = hashedPassword.Item1;
            string passwordSalt = hashedPassword.Item2;
            UserIdentity userIdentity = GetUserIdentity(registerDto, passwordHash, passwordSalt);
            bool userExists = await UserExistsAsync(registerDto.Email);
            if (!userExists)
            {
                _appDbContext.UserIdentities.Add(userIdentity);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Kullanıcı zaten mevcut");
            }
        }
        public async Task<TokenDto> Login(LoginDto loginDto)
        {
            string password = loginDto.Password;
            string email = loginDto.Email;
            DateTime tokenExpiryDate = DateTime.UtcNow.AddHours(1);
            UserIdentity? user = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if(user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }
            else 
            {
                bool passwordVerified = PasswordService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
                if (!passwordVerified)
                {
                    throw new Exception("Şifre geçerli değil");

                }
                else
                {
                    string token = _tokenService.GenerateToken(user.UserName, user.Email, tokenExpiryDate);
                    return new TokenDto() { Token = token, ExpiryDate = tokenExpiryDate };
                }

            }

          

        }
        public UserIdentity GetUserIdentity(RegisterDto registerDto, string passwordHash, string passwordSalt)
        {
            return new UserIdentity()
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserName = registerDto.UserName
            };
        }
    }
}
