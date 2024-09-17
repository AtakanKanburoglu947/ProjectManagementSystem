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
    public class JobsController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<Job, JobDto, JobUpdateDto> _jobService;
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly IService<Project,ProjectDto, ProjectUpdateDto> _projectService;
       
        private readonly NotificationService _notificationService;

        public JobsController(AuthService authService,
            IService<Job,JobDto, JobUpdateDto> jobService,
            IService<Project, ProjectDto, ProjectUpdateDto> projectService,
            NotificationService notificationService,
            IService<Manager, ManagerDto, ManagerUpdateDto> managerService)
        {
            _authService = authService;
            _jobService = jobService;
            _projectService = projectService;
            _notificationService = notificationService;
            _managerService = managerService;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                id *= 5;
            }
            Guid userIdentityId = await _authService.GetUserIdentityId();
            Manager manager = await _managerService.Get(x => x.UserIdentityId == userIdentityId);
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);
            List<Job> jobs = new List<Job>();
            if (manager != null)
            {
                ViewData["manager"] = true;
                jobs = await _jobService.Filter(id,x=>(DateTime)x.AddedAt!,x=>x.ManagerId == manager.Id);
            }
            else { 
                 jobs = await _jobService.Filter(id,x=>(DateTime)x.AddedAt!,x=>x.UserIdentityId == userIdentityId);
            
            }
            List<JobPageModel> jobPageModels = new List<JobPageModel>();
            PaginationModel<JobPageModel, NoData> paginationModel = new PaginationModel<JobPageModel, NoData>();
            if (jobs != null)
            {
                int count = 0;
                if (manager != null)
                {
                    
                    count = _jobService.Count(x=>x.ManagerId == manager.Id);
                }
                else
                {
                    count = _jobService.Count(x=>x.UserIdentityId == userIdentityId);
                }
                foreach (var job in jobs)
                {
                    var project = await _projectService.Get(x => x.Id == job.ProjectId);
                    JobPageModel jobPageModel = new JobPageModel()
                    {
                        Id = job.Id,
                        ProjectId = job.ProjectId,
                        Title = job.Title,
                        Description = job.Description,
                        ProjectName = project.Name,
                        UserId = job.UserId,
                        UserIdentityId = userIdentityId,
                        FileUploadId = job.FileUploadId,
                        Time = job.DueDate - DateTime.Now,
                        AddedAt = job.AddedAt
                    };
                    jobPageModels.Add(jobPageModel);

                }
                paginationModel = Pagination<JobPageModel, NoData>.Model(id, userIdentityId, null, jobPageModels, count);
            }

            return View(paginationModel);
        }
    }
}
