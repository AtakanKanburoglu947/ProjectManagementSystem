using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    public class ProjectsController : Controller
    {
        IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        private readonly AuthService _authService;
        private readonly IService<ProjectUser, ProjectUser, ProjectUser> _projectUserService;
        private readonly IService<User, UserDto, UserUpdateDto> _userService;

        public ProjectsController(IService<Project, ProjectDto, ProjectUpdateDto> projectService, IService<ProjectUser, ProjectUser, ProjectUser> projectUserService
    , AuthService authService, IService<User, UserDto, UserUpdateDto> userService)
        {
            _projectService = projectService;
            _projectUserService = projectUserService;
            _authService = authService;
            _userService = userService;

        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();

            User? user = await _userService.Get(x => x.UserIdentityId == userIdentityId);
            List<ProjectUser>? userProjects = _projectUserService.Where(x => x.UserId == user.Id);
            List<Guid> projectIds = userProjects.Select(x => x.ProjectId).Distinct().ToList();
            List<Project>? projects = _projectService.Where(x => projectIds.Contains(x.Id));
            return View(projects);
        }
    }
}
