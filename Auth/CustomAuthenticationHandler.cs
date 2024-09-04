using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Auth
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TimeProvider _timeProvider; 
        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                    ILoggerFactory logger,
                                    UrlEncoder encoder,
                                    
                                    TimeProvider timeProvider)
    : base(options, logger, encoder, new SystemClock())
        {            _timeProvider = timeProvider;

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Cookies.TryGetValue("token",out string? token) == false) {
                return  AuthenticateResult.Fail("Cookie bulunamadı");
            }
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                Claim[] claims = jwtToken.Claims.ToArray();
                ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception exception)
            {
                return AuthenticateResult.Fail($"Token hatası: {exception.Message}");
                throw;
            }
            

        }
    }
}
