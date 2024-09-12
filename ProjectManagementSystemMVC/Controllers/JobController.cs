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
    public class JobController : Controller
    {
        private readonly IService<Job, JobDto, JobUpdateDto> _jobService;
        private readonly IService<Project,ProjectDto, ProjectUpdateDto> _projectService;
        private readonly FileService _fileService;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        public JobController(IService<Job,JobDto,JobUpdateDto> jobService, 
            IService<Project,ProjectDto,ProjectUpdateDto> projectService,
            FileService fileService, AuthService authService, NotificationService notificationService)
        {
            _jobService = jobService;
            _projectService = projectService;
            _fileService = fileService;
            _authService = authService;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index(Guid id)
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            Job job = await _jobService.Get(id);
            Project project = await _projectService.Get(job.ProjectId);
            FileUpload? file = await _fileService.GetFile((Guid)job.FileUploadId); 
            JobPageModel jobPageModel = new JobPageModel()
            {
                Id = job.Id,
                Title = job.Title,
                ProjectId = job.ProjectId,
                UserId = job.UserId,
                Status = job.Status,
                Description = job.Description,
                UserIdentityId = job.UserIdentityId,
                FileUploadId = job.FileUploadId,
                Time = job.DueDate - DateTime.Now,
                ProjectName = project.Name,
                FileName = file.Name,
                AddedAt = job.AddedAt
            };
            return View(jobPageModel);
        }
        public async Task<IActionResult> Download(Guid id)
        {
            FileUpload? file = await _fileService.GetFile(id);
            return File(file.Data, "application/octet-stream", fileDownloadName: file.Name);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateJobStatus(Guid jobId, string newStatus)
        {
            Job job = await _jobService.Get(jobId);
            JobUpdateDto jobUpdateDto = new JobUpdateDto() {
               Id = job.Id,
               Description = job.Description,
               DueDate = job.DueDate,
               Status = newStatus,
               Title = job.Title,
               UserId= job.UserId,
               FileUploadId= job.FileUploadId,
               ProjectId= job.ProjectId,
               UserIdentityId= job.UserIdentityId
            };
            await _jobService.Update(jobUpdateDto, jobId);
            return Redirect($"/Job/Index/{jobId}");
        }
    }
}
