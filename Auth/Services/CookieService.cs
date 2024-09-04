using Microsoft.AspNetCore.Http;


namespace Auth
{
    public static class CookieService
    {
        public static void SetCookie(string key, string value,DateTimeOffset expires, IHttpContextAccessor contextAccessor) {
            CookieOptions options = new CookieOptions() {Expires = expires};
            contextAccessor?.HttpContext?.Response.Cookies.Append(key,value,options);
        }
        public static void RemoveCookie(string key, IHttpContextAccessor contextAccessor)
        {
            var cookie = GetCookie(key, contextAccessor);

            contextAccessor.HttpContext.Response.Cookies.Delete(key);
            cookie = GetCookie(key,contextAccessor);
        }
        public static string GetCookie(string key, IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor?.HttpContext == null) return null;

            return contextAccessor.HttpContext.Request.Cookies[key];
        }
    }
}
