using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class ProjectAccessFilter : IAsyncActionFilter
    {
        private readonly IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        private readonly AuthService _authService;
        private readonly IService<User, UserDto, UserUpdateDto> _userService;
        private readonly IService<ProjectUser, ProjectUser, ProjectUser> _projectUserService;

        public ProjectAccessFilter(
             IService<Project, ProjectDto, ProjectUpdateDto> projectService,
            AuthService authService,
            IService<User, UserDto, UserUpdateDto> userService,
            IService<ProjectUser, ProjectUser, ProjectUser> projectUserService)
        {
            _projectService = projectService;
            _authService = authService;
            _userService = userService;
            _projectUserService = projectUserService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid? id = (Guid)context.ActionArguments["id"]!;
            Guid identityId = await _authService.GetUserIdentityId();
            User user = await _userService.Get(x => x.UserIdentityId == identityId);
            List<ProjectUser> projectsofUser = _projectUserService.Where(x => x.UserId == user.Id);
            if (!projectsofUser.Any(x=>x.ProjectId== id))
            {
                context.Result = new NotFoundResult();
                return;
            }
            await next();

        }
    }
}
