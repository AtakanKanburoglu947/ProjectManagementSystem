using Auth.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementSystemCore.Models;

namespace Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[] _roles;
        public RoleAuthorizeAttribute(string[] roles)
        {
            _roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            AuthService? authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();
            string userRole = await authService.GetUserRole(context.HttpContext.Request);
            foreach (string role in _roles)
            {
                if (userRole == role)
                {
                    await next();
            
                }
            }
            context.Result = new ForbidResult();
            return;
        }
    }
}
