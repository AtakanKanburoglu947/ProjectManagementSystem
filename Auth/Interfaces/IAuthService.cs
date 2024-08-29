using Microsoft.AspNetCore.Http;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto registerDto);
        Task<bool> UserExistsAsync(string email);
        bool PasswordVerified(string password, UserIdentity user);
        UserIdentity GetUserIdentity(RegisterDto registerDto, string passwordHash, string passwordSalt);
        Task<TokenDto> Login(LoginDto loginDto);
    }
}
