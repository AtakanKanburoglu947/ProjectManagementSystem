using Auth;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemRepository;
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
        private readonly AppDbContext _appDbContext;
        public JobController(IService<Job,JobDto,JobUpdateDto> jobService, 
            IService<Project,ProjectDto,ProjectUpdateDto> projectService,
            FileService fileService, AuthService authService, NotificationService notificationService, 
            IService<Manager,ManagerDto,ManagerUpdateDto> managerService, IService<User,UserDto,UserUpdateDto> userService, IService<Message,MessageDto,MessageDto> messageService
            , CacheService cacheService, AppDbContext appDbContext)
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
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index(Guid id)
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            Manager manager = await _managerService.Get(x => x.UserIdentityId == userIdentityId);
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);
            if (manager != null)
            {
                ViewData["manager"] = true;
            }
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
            ViewData["notifications"] = await _notificationService.GetNotifications(await _authService.GetUserIdentityId());
            if (ModelState.IsValid)
            {
                if (_projectService.FirstOrDefault(x => x.Name == jobPageModel.ProjectName) == null)
                {
                    ModelState.AddModelError(jobPageModel.ProjectName,"Proje adı geçerli değil");

                }
                if (_authService.FirstOrDefault(x => x.Email == jobPageModel.UserName) == null)
                {
                    ModelState.AddModelError(jobPageModel.UserName, "Kullanıcı adı geçerli değil");

                }
                Guid managerIdentityId = await _authService.GetUserIdentityId();
                UserIdentity managerIdentity = await _authService.GetUserById(managerIdentityId);
                Manager manager = await _managerService.Get(x => x.UserIdentityId == managerIdentityId);
                Guid userIdentityId = await _authService.GetUserIdentityId(jobPageModel.UserName);
                User user = await _userService.Get(x => x.UserIdentityId == userIdentityId);
                Project project = await _projectService.Get(x => x.Name == jobPageModel.ProjectName);

                JobDto jobDto = new JobDto();

                jobDto.AddedAt = DateTime.Now;
                jobDto.Status = jobPageModel.StatusOptions[0];
                    jobDto.ManagerId = manager.Id;
                jobDto.Description = jobPageModel.Description;
                jobDto.DueDate = jobPageModel.DueDate;
                jobDto.Title = jobPageModel.Title;
                jobDto.UserId = user.Id;
                jobDto.UserIdentityId = userIdentityId;
                jobDto.ProjectId = project.Id;
                


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
                await _messageService.Add(messageDto);
                _cacheService.SetClass("messages", user.UserIdentityId, async () => await _messageService.Filter(0, x => (DateTime)x.AddedAt, x => x.ReceiverId == user.UserIdentityId), TimeSpan.FromHours(1), TimeSpan.FromMinutes(20));
                await _notificationService.Notify(user.UserIdentityId);
                return View("Add", jobPageModel);
            }
            else 
            {
                return View("Add", jobPageModel);

            }

        }
        [RoleAuthorize(["Manager"])]
        public async Task<IActionResult> Update(Guid id)
        {
            var job = await _jobService.Get(id);
            Guid userIdentityId = await _authService.GetUserIdentityId();

            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            JobPageModel jobPageModel = new JobPageModel()
            {
                AddedAt= job.AddedAt,
                Description = job.Description,
                DueDate = job.DueDate,
                FileUploadId = job.FileUploadId,
                Id = id,
                ManagerId = job.ManagerId,
                ProjectId = job.ProjectId,
                Status = job.Status,
                Title = job.Title,
                UserIdentityId = job.UserIdentityId,
                UserId = job.UserId,
                
            };
            return View(jobPageModel);
        }
        [RoleAuthorize(["Manager"])]
        public async Task<IActionResult> UpdateJob(JobPageModel jobPageModel)
        {
            bool descriptionIsEmpty = string.IsNullOrEmpty(jobPageModel.Description);
            if (!descriptionIsEmpty)
            {
                var job = await _jobService.Get(jobPageModel.Id);
                job.Description = jobPageModel.Description;
                job.DueDate = jobPageModel.DueDate;
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Index", new { id = jobPageModel.Id });

            }
            else
            {
                return RedirectToAction("Update", new { id = jobPageModel.Id });

            }
        }
    }
}
