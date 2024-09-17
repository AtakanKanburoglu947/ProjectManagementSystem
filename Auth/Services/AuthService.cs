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
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystemService;
using ProjectManagementSystemCore;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Services
{
    public class AuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CacheService _cacheService;
        private readonly IService<User, UserDto, UserUpdateDto> _service;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext appDbContext, TokenService tokenService, IHttpContextAccessor contextAccessor, IService<User,UserDto,UserUpdateDto> service, IMapper mapper, CacheService cacheService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _contextAccessor = contextAccessor;
            _service = service;
            _mapper = mapper;
            _cacheService = cacheService;
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
                UserDto userDto = new UserDto() { RoleId = Roles.User.Id, UserIdentityId =  userIdentity.Id };
                _appDbContext.Users.Add(_mapper.Map<User>(userDto));
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
                    CookieService.SetCookie("token", token, DateTimeOffset.UtcNow.AddHours(1), _contextAccessor);
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
                return null;
            }
        } 
        private object GetEmailFromCookie()
        {
           
            string? token = CookieService.GetCookie("token", _contextAccessor);
            if (token != null)
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtSecurityTokenHandler.ReadToken(token) as JwtSecurityToken;
                JwtPayload payload = jwtToken?.Payload!;
                return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            }
            throw new Exception("Token bulunamadı");

            
        }

        public UserIdentity FirstOrDefault(Expression<Func<UserIdentity, bool>> expression)
        {
            UserIdentity userIdentity = _appDbContext.UserIdentities.FirstOrDefault(expression);
            if (userIdentity != null)
            {
                return userIdentity;
            }
            return null;
        }

        public async Task<string> UpdatePassword(PasswordDto passwordDto)
        {

            string? email = GetEmailFromCookie() as string;    
            UserIdentity? userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x=>x.Email == email);
            if (userIdentity != null)
                {
                try
                {
                    return await UpdatePasswordHashAndSalt(passwordDto, userIdentity);
                }
                catch (Exception exception)
                {
                    
                    throw new Exception("Eski şifre gerekli");
                }

            }
            else
                {
                  throw new Exception("Kullanıcı bulunamadı");
                }
        }
  
        public async Task<string> GetUserRole(HttpRequest request)
        {
            string? email = GetEmailFromCookie() as string;

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
            Manager? manager = await _appDbContext.Managers.FirstOrDefaultAsync(x=>x.UserIdentityId == userIdentity.Id);
            if (manager != null)
            {
                Role? userRole = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Id == manager.RoleId);
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
        public async Task<UserIdentity> GetUserById(Guid id)
        {
            var userIdentity = await _appDbContext.UserIdentities.FindAsync(id); 
            if (userIdentity != null)
            {
                return userIdentity;
            }
            return null;
        }
        public async Task<UserIdentity> GetUserByEmail(string email)
        {
            var userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email == email);
            if (userIdentity != null)
            {
                return userIdentity;
            }
            return null;
        }

        public async Task<Guid> GetUserIdentityId()
        {
            string? email = GetEmailFromCookie() as string;
            try
            {
                var userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x=>x.Email == email);
                return userIdentity!.Id;
            }
            catch (Exception)
            {

                throw new Exception("Id bulunamadı");
            }
        }

        public async Task<Guid> GetUserIdentityId(string email)
        {
            var userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email == email);
            if (userIdentity != null)
            {
                return userIdentity!.Id;
            }
            return Guid.Empty;
         
        }
        public async Task<string> GetUserName()
        {
            string? email = GetEmailFromCookie() as string;
            try
            {
                var userIdentity = await _appDbContext.UserIdentities.FirstOrDefaultAsync(x => x.Email == email);
                return userIdentity!.UserName;
            }
            catch (Exception)
            {

                throw new Exception("Id bulunamadı");
            }
        }
        public async Task<UserIdentity> GetUserIdentity(HttpRequest request)
        {
            var id = await GetUserIdentityId();
            var userIdentity = _appDbContext.UserIdentities.FirstOrDefault(x => x.Id == id);
            return userIdentity;
        }
        public List<UserIdentity> Where(Expression<Func<UserIdentity, bool>> expression)
        {
            List<UserIdentity> result = _appDbContext.UserIdentities.Where(expression).ToList();
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public void Logout()
        {
            CookieService.RemoveCookie("token", _contextAccessor);
        }

        public string GetToken()
        {
            return CookieService.GetCookie("token", _contextAccessor);
        }
    }
}