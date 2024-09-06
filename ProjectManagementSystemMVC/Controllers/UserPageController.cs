using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<ProjectUser,ProjectUser,ProjectUser> _projectUserService;
        private readonly IService<User,UserDto,UserUpdateDto> _userService;
        private readonly IService<Project, ProjectDto, ProjectUpdateDto> _projectService;

        public UserPageController(AuthService authService, IService<ProjectUser,ProjectUser,ProjectUser> projectUserService,
                IService<User, UserDto, UserUpdateDto> userService,
                IService<Project,ProjectDto,ProjectUpdateDto> projectService
            )
        {
            _authService = authService;
            _projectUserService = projectUserService;
            _userService = userService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            string? userName = await _authService.GetUserName();
            User? user = await _userService.Get(x=>x.UserIdentityId == userIdentityId);
            List<ProjectUser>? userProjects = await _projectUserService.Where(x=>x.UserId == user.Id,"projectusers");
           
            UserDto userDto = new UserDto() { RoleId = user.Id, UserIdentityId = userIdentityId };
            UserPageModel userPageModel = new UserPageModel();
            if (userProjects.Count > 0)
            {
                List<Guid> projectIds = userProjects.Select(x => x.ProjectId).Distinct().ToList();
                List<Project>? projects = await _projectService.Where(x=>projectIds.Contains(x.Id),"projects");
                userPageModel.UserName = userName;
                userPageModel.Projects = projects;

            }
            else
            {
                userPageModel.UserName = userName;
            }
            return View(userPageModel);
        }
        public IActionResult Logout()
        {
            _authService.Logout();
            return Redirect("/Login");
        }
    }
}
