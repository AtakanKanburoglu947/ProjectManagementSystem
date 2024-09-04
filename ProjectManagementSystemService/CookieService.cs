using Microsoft.AspNetCore.Http;


namespace ProjectManagementSystemService
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CookieService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void SetCookie(string key, string value,DateTimeOffset? expires) {
            CookieOptions options = new CookieOptions()
            {
                SameSite = SameSiteMode.Strict,
                Path = "/",
                
            };
            if (expires.HasValue)
            {
                options.Expires = expires.Value;
            }
            _contextAccessor.HttpContext.Response.Cookies.Append(key,value,options);
        }
    }
}
