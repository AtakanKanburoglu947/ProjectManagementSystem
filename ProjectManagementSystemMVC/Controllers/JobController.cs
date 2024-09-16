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
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly IService<User,UserDto, UserUpdateDto> _userService;
        private readonly IService<Message, MessageDto, MessageDto> _messageService;
        private readonly FileService _fileService;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;
        private readonly CacheService _cacheService;
        public JobController(IService<Job,JobDto,JobUpdateDto> jobService, 
            IService<Project,ProjectDto,ProjectUpdateDto> projectService,
            FileService fileService, AuthService authService, NotificationService notificationService, 
            IService<Manager,ManagerDto,ManagerUpdateDto> managerService, IService<User,UserDto,UserUpdateDto> userService, IService<Message,MessageDto,MessageDto> messageService
            , CacheService cacheService)
        {
            _jobService = jobService;
            _projectService = projectService;
            _fileService = fileService;
            _authService = authService;
            _notificationService = notificationService;
            _managerService = managerService;
            _userService = userService;
            _messageService = messageService;
            _cacheService = cacheService;
        }
        public async Task<IActionResult> Index(Guid id)
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            Job job = await _jobService.Get(id);
            Project project = await _projectService.Get(job.ProjectId);
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
                AddedAt = job.AddedAt
            };
            if (job.FileUploadId != null)
            {
                FileUpload file = await _fileService.GetFile((Guid)job.FileUploadId);

                jobPageModel.FileName = file.Name;
            }

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
            Guid userIdentityId = await _authService.GetUserIdentityId();
            UserIdentity userIdentity =await  _authService.GetUserById(userIdentityId);
            User user = await _userService.Get(x=>x.UserIdentityId==userIdentityId);
            Job job = await _jobService.Get(jobId);
            Manager manager = await _managerService.Get(x=>x.Id == job.ManagerId);
            JobUpdateDto jobUpdateDto = new JobUpdateDto() {
               Id = job.Id,
               Description = job.Description,
               DueDate = job.DueDate,
               Status = newStatus,
               Title = job.Title,
               UserId= job.UserId,
               FileUploadId= job.FileUploadId,
               ProjectId= job.ProjectId,
               UserIdentityId= job.UserIdentityId,
               ManagerId = manager.Id
            };
            

            await _jobService.Update(jobUpdateDto, jobId);
            return Redirect($"/Job/Index/{jobId}");
        }
        public async Task<IActionResult> Add(JobPageModel jobPageModel)
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            return View(jobPageModel);  
        }
        [HttpPost]
        public async Task<IActionResult> AddJob(JobPageModel jobPageModel)
        {
            Guid managerIdentityId = await _authService.GetUserIdentityId();
            UserIdentity managerIdentity = await _authService.GetUserById(managerIdentityId);
            Manager manager = await _managerService.Get(x=>x.UserIdentityId == managerIdentityId);
            Guid userIdentityId = await _authService.GetUserIdentityId(jobPageModel.UserName);
            User user = await _userService.Get(x=>x.UserIdentityId == userIdentityId);
            Project project = await _projectService.Get(x => x.Name == jobPageModel.ProjectName);
            JobDto jobDto = new JobDto
            {
                AddedAt = DateTime.Now,
                Status = jobPageModel.StatusOptions[0],
                ManagerId = manager.Id,
                Description = jobPageModel.Description,
                DueDate = jobPageModel.DueDate,
                Title = jobPageModel.Title,
                UserId = user.Id,
                UserIdentityId = userIdentityId,
                ProjectId = project.Id
            };


            await _jobService.Add(jobDto);
            Job job = await _jobService.Get(x => x.Title == jobDto.Title);
            string url = $"/Job/Index/{job.Id}/";
            MessageDto messageDto = new MessageDto()
            {
                AddedAt = DateTime.Now,
                Content = $" {managerIdentity.UserName} sana yeni bir iş verdi.  <a href = \"{url}\"> Tıkla </a>",
                Name = "Yeni İş",
                ReceiverId = user.UserIdentityId,
                SenderId = manager.UserIdentityId
            };
            await  _messageService.Add(messageDto);
            _cacheService.SetClass("messages", user.UserIdentityId, async () => await _messageService.Filter(0,x=>(DateTime)x.AddedAt,x=>x.ReceiverId == user.UserIdentityId), TimeSpan.FromHours(1), TimeSpan.FromMinutes(20));
            await _notificationService.Notify(user.UserIdentityId);
            return RedirectToAction("Add","Job",jobPageModel);
        }
    }
}
