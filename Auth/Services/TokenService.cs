﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class TokenService
    {
        private readonly string _secretKey;
        public TokenService(string secretKey)
        {
            _secretKey = secretKey;
        }
        public string GenerateToken(string userName, string email, DateTime expiryDate)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = GetClaims(userName, email);
            var token = GetJwtSecurityToken(claims, expiryDate, creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private JwtSecurityToken GetJwtSecurityToken(Claim[] claims, DateTime expiryDate, SigningCredentials creds)
        {

            return new JwtSecurityToken(
                issuer: "https://localhost:7038",
                audience: "User",
                claims: claims,
                expires: expiryDate,
                signingCredentials: creds);
        }
        public static Claim[] GetClaims(string userName, string email)
        {
            return
            [
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email),
            ];

        }
        public TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        }
  
    }
}
