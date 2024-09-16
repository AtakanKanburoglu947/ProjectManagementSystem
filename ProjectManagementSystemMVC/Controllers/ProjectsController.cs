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
    public class ProjectsController : Controller
    {
        IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        private readonly AuthService _authService;
        private readonly IService<ProjectUser, ProjectUser, ProjectUser> _projectUserService;
        private readonly IService<ProjectManager, ProjectManager, ProjectManager> _projectManagerService;
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly IService<User, UserDto, UserUpdateDto> _userService;
        private readonly NotificationService _notificationService;


        public ProjectsController(IService<Project, ProjectDto, ProjectUpdateDto> projectService, IService<ProjectUser, ProjectUser, ProjectUser> projectUserService
    , AuthService authService, IService<User, UserDto, UserUpdateDto> userService, NotificationService notificationService, 
            IService<Manager,ManagerDto,ManagerUpdateDto> managerService,IService<ProjectManager,ProjectManager,ProjectManager> projectManagerService)
        {
            _projectService = projectService;
            _projectUserService = projectUserService;
            _authService = authService;
            _userService = userService;
            _notificationService = notificationService;
            _managerService = managerService;
            _projectManagerService = projectManagerService;

        }
        public async Task<IActionResult> Index(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                id *= 5;
            }
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);
            User? user = await _userService.Get(x => x.UserIdentityId == userIdentityId);
            List<Project>? projects = new List<Project>();
            PaginationModel<Project, NoData> paginationModel = new PaginationModel<Project, NoData>();
            if (user != null)
            {
                List<ProjectUser>? userProjects = await _projectUserService.Filter(id, x => x.UserId == user.Id);

                if (userProjects.Any())
                {
                    int count = _projectUserService.Count(x => x.UserId == user.Id);
                    List<Guid> projectIds = userProjects.Select(x => x.ProjectId).Distinct().ToList();
                    projects = _projectService.Where(x => (DateTime)x.AddedAt, x => projectIds.Contains(x.Id));
                    paginationModel = Pagination<Project, NoData>.Model(id, userIdentityId, null, projects, count);
                }
            }
            Manager manager = await _managerService.Get(x => x.UserIdentityId == userIdentityId);
            if (manager != null)
            {
                ViewData["manager"] = true;
                List<ProjectManager>? managerProjects = await _projectManagerService.Filter(id, x => x.ManagerId == manager.Id);
                if (managerProjects.Any())
                {
                    int count = _projectManagerService.Count(x => x.ManagerId == manager.Id);
                    List<Guid> projectIds = managerProjects.Select(x => x.ProjectId).Distinct().ToList();
                    projects = _projectService.Where(x => (DateTime)x.AddedAt, x => projectIds.Contains(x.Id));
                    paginationModel = Pagination<Project, NoData>.Model(id, userIdentityId, null, projects, count);

                }   
            }
            return View(paginationModel);
        }
    }
}
