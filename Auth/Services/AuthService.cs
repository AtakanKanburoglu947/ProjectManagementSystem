using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;

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
            UserIdentity userIdentity = new UserIdentity()
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserName = registerDto.UserName
            };
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
            if (user == null)
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
        private async Task<string> UpdatePasswordHashAndSalt(PasswordDto passwordDto, UserIdentity userIdentity)
        {
            if (PasswordVerified(passwordDto.OldPassword, userIdentity))
            {
                (string, string) newPassword = PasswordService.HashPassword(passwordDto.NewPassword);
                string newPasswordHash = newPassword.Item1;
                string newPasswordSalt = newPassword.Item2;

                userIdentity.PasswordSalt = newPasswordSalt;
                userIdentity.PasswordHash = newPasswordHash;
                await _appDbContext.SaveChangesAsync();
                return "Şifre güncellendi";
            }
            else
            {
                throw new Exception("Eski şifre gerekli");
            }
        } 
        private object GetEmailFromAuthorizationHeader(HttpRequest request)
        {
            string? authorizationHeader = request.Headers.Authorization.FirstOrDefault();
            string bearer = "Bearer ";
            if (authorizationHeader != null && authorizationHeader.StartsWith(bearer))
            {
                string? token = authorizationHeader.Substring(bearer.Length);
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtSecurityTokenHandler.ReadToken(token) as JwtSecurityToken;
                JwtPayload payload = jwtToken?.Payload!;
                return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            }
            throw new Exception("Email bulunamadı");
        }
        public async Task<string> UpdatePassword(HttpRequest request, PasswordDto passwordDto)
        {

            string? email = GetEmailFromAuthorizationHeader(request) as string;    
            UserIdentity? userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x=>x.Email == email);
            if (userIdentity != null)
                {
                  return await UpdatePasswordHashAndSalt(passwordDto, userIdentity);
                }
            else
                {
                  throw new Exception("Kullanıcı bulunamadı");
                }      
        }
        public async Task<string> GetUserRole(HttpRequest request)
        {
            string? email = GetEmailFromAuthorizationHeader(request) as string;

            UserIdentity? userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email == email);
            var id = userIdentity.Id;
            User? user = await _appDbContext.Users.FirstOrDefaultAsync(x=>x.UserIdentityId == userIdentity.Id);
            if (user != null)
            {
                
                Role? userRole = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
                if (userRole != null)
                {
                    return userRole!.Title;
                }
                else
                {
                    throw new Exception("Kullanıcı rolü bulunamadı");

                }

            }
            throw new Exception("Kullanıcı bulunamadı");
        } 

    }
}