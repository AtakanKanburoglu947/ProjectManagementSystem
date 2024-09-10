using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    public class JobsController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<Job, JobDto, JobUpdateDto> _jobService;
        private readonly IService<Project,ProjectDto, ProjectUpdateDto> _projectService;
        public JobsController(AuthService authService,
            IService<Job,JobDto, JobUpdateDto> jobService,
            IService<Project, ProjectDto, ProjectUpdateDto> projectService)
        {
            _authService = authService;
            _jobService = jobService;
            _projectService = projectService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            List<Job> jobs = _jobService.Where(x=>x.UserIdentityId == userIdentityId).ToList();
            List<JobPageModel> jobPageModels = new List<JobPageModel>();
            foreach (var job in jobs)
            {
                var project = await  _projectService.Get(x => x.Id == job.ProjectId);
                JobPageModel jobPageModel = new JobPageModel()
                {
                    Id = job.Id,
                    ProjectId = job.ProjectId,
                    Title = job.Title,
                    Description = job.Description,
                    Status = job.Status,
                    ProjectName =  project.Name,
                    UserId = job.UserId,
                    UserIdentityId = userIdentityId,
                    FileUploadId = job.FileUploadId,
                    Time = job.DueDate - DateTime.Now
                };
                jobPageModels.Add(jobPageModel);

            }
            return View(jobPageModels);
        }
    }
}
