using Auth;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;
using System.Linq.Expressions;
using System.Text.Json;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly AuthService _authService;
        private readonly FileService _fileService;
        private readonly IService<Project,ProjectDto,ProjectUpdateDto> _projectService;
        private readonly IService<ProjectUser, ProjectUser, ProjectUser> _projectUserService;
        private readonly IService<ProjectManager, ProjectManager, ProjectManager> _projectManagerService;
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly IService<User, UserDto, UserUpdateDto> _userService;
        private readonly IService<Message, MessageDto, MessageDto> _messageService;
        private readonly IService<Comment,CommentDto, CommentUpdateDto> _commentService;
        private readonly NotificationService _notificationService;
        private readonly CacheService _cacheService;

        public ProjectController(AuthService authService, IService<Project, ProjectDto, ProjectUpdateDto> projectService
        , IService<ProjectUser, ProjectUser, ProjectUser> projectUserService, IService<User, UserDto, UserUpdateDto> userService
            , IService<ProjectManager, ProjectManager, ProjectManager> projectManagerService, IService<Manager, ManagerDto, ManagerUpdateDto> managerService,
            IService<Comment, CommentDto, CommentUpdateDto> commentService, FileService fileService, NotificationService notificationService, CacheService cacheService,
            IService<Message, MessageDto, MessageDto> messageService
            )
        {
            _authService = authService;
            _projectService = projectService;
            _projectUserService = projectUserService;
            _userService = userService;
            _projectManagerService = projectManagerService;
            _managerService = managerService;
            _commentService = commentService;
            _fileService = fileService;
            _notificationService = notificationService;
            _cacheService = cacheService;
            _messageService  = messageService;
        }
        public async Task<IActionResult> Index(Guid projectId,int startIndex)
        {
            ViewData["startIndex"] = startIndex;
            if (startIndex > 0)
            {
                startIndex *= 5;

            }
            ViewData["projectId"] = projectId;
            TempData["startIndex"] = startIndex;
            Project project = await _projectService.Get(projectId);      
            List<ProjectUser> projectUsers = _projectUserService.Where(x=> x.ProjectId == project.Id); 
            List<ProjectManager> projectManagers = _projectManagerService.Where(x=>x.ProjectId == project.Id);
            List<int> managerIds = projectManagers.Select(x=>x.ManagerId).Distinct().ToList();
            List<int> userIds = projectUsers.Select(x=>x.UserId).Distinct().ToList();
            List<Manager> managers = _managerService.Where(x => managerIds.Contains(x.Id));
            List<User> users = _userService.Where(x => userIds.Contains(x.Id));
            List<Guid> userIdentityIds = users.Select(x=>x.UserIdentityId).Distinct().ToList();
            List<Guid> managerIdentityIds = managers.Select(x => x.UserIdentityId).Distinct().ToList();
            List<UserIdentity> managerIdentities = new List<UserIdentity>();
            Guid userIdentityId = await _authService.GetUserIdentityId();
            Expression<Func<Comment, DateTime>> expression = x => (DateTime)x.AddedAt;
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);


            List<Comment> comments = await _commentService.Filter(startIndex    ,expression,x=>x.ProjectId == projectId);
            int count = _commentService.Count(x => x.ProjectId == projectId);
            List<CommentDetails> commentDetails = new List<CommentDetails>();

            foreach (var item in managerIdentityIds)
            {
                managerIdentities.Add(await _authService.GetUserById(item));
            }

            ProjectPageModel projectPageModel = new ProjectPageModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                Version = project.Version,
                ManagerIdentities = managerIdentities,
                UserIdentityId = await _authService.GetUserIdentityId(),
                AddedAt = project.AddedAt
                
            };
            if (userIdentityIds.Count > 0)
            {
                List<UserIdentity> userIdentities = new List<UserIdentity>();
                foreach (var userId in userIdentityIds)
                {
                    userIdentities.Add(await _authService.GetUserById(userId));
                }
                projectPageModel.UserIdentities = userIdentities;
                
            }
            if (comments.Count > 0)
            {   
               
                foreach (var comment in comments)
                { 
                    var _user = await _authService.GetUserById(comment.UserIdentityId);
                    var commentDetail = new CommentDetails()
                    {
                        ProjectName = project.Name,
                        ProjectId = project.Id,
                        CommentId = comment.Id,
                        Text = comment.Text,
                        UserName = _user.UserName,
                        UserIdentityId = _user.Id,
                        AddedAt = comment.AddedAt
                        
                    };
                    if (comment.FileUploadId != null)
                    {
                        var commentFile = await _fileService.GetFile((Guid)comment.FileUploadId);
                        commentDetail.FileId = commentFile.Id;
                        commentDetail.FileName = commentFile.Name;
                    }
                    if (comment.UpdatedAt != null)
                    {
                        commentDetail.UpdatedAt = comment.UpdatedAt;
                    }
                    commentDetails.Add(commentDetail);
                }
              

            }
            PaginationModel<CommentDetails, ProjectPageModel> paginationModel = Pagination<CommentDetails, ProjectPageModel>.Model(startIndex, userIdentityId, projectPageModel,
                  commentDetails, count);
            return View(paginationModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid projectId, CommentDetails newComment, IFormFile file)
        {
            if (string.IsNullOrEmpty(newComment.Text))
            {
                return Redirect($"/Project/Index/{projectId}");

            }
            Guid userIdentityId = await _authService.GetUserIdentityId();
            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            Project project = await _projectService.Get(projectId);
            var commentDto = new CommentDto()
            {
                ProjectId = projectId,
                Text = newComment.Text,
                UserIdentityId = userIdentityId,
                AddedAt = DateTime.Now

            };
            if (file != null && file.Length >0) {
                Guid fileUploadId = await _fileService.Upload(file, [".txt"], await _authService.GetUserIdentityId(),null);
                commentDto.FileUploadId = fileUploadId;
            }
            await _commentService.Add(commentDto);
            
            TimeSpan absoluteExpiration = TimeSpan.FromHours(1);
            TimeSpan slidingExpiration = TimeSpan.FromMinutes(20);
            _cacheService.SetClass("files",commentDto.UserIdentityId, await _fileService.GetFilesOfUser(userIdentityId), absoluteExpiration, slidingExpiration);
            _cacheService.SetStruct("filesCount",commentDto.UserIdentityId, _fileService.Count(x=>x.UserIdentityId == userIdentityId), absoluteExpiration, slidingExpiration);
            var startIndex = TempData["startIndex"];
            return Redirect($"Index?startIndex={startIndex}&projectId={projectId}");

        }
        public async Task<IActionResult> RemoveComment(Guid commentId, Guid projectId)
        {
            var comment = await _commentService.Get(commentId);
            Guid userIdentityId = await _authService.GetUserIdentityId();
            await _commentService.Remove(x => x.Id == commentId);
            if (comment.FileUploadId != null || comment.FileUploadId != Guid.Empty)
            {
                await _fileService.RemoveFile(comment.FileUploadId);
                TimeSpan absoluteExpiration = TimeSpan.FromHours(1);
                TimeSpan slidingExpiration = TimeSpan.FromMinutes(20);
                _cacheService.SetClass("files",userIdentityId, await _fileService.GetFilesOfUser(userIdentityId), absoluteExpiration, slidingExpiration);
                _cacheService.SetStruct("filesCount",userIdentityId, _fileService.Count(x => x.UserIdentityId == userIdentityId), absoluteExpiration, slidingExpiration);
            }
            var startIndex = TempData["startIndex"];

            return Redirect($"Index?startIndex={startIndex}&projectId={projectId}");
        }
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentDto commentDto,Guid commentId, Guid projectId, Guid userIdentityId)
        {
            var comment = await _commentService.Get(commentId);
            var commentUpdateDto = new CommentUpdateDto() {
              Id = commentId,
              ProjectId =  projectId,
              Text = commentDto.Text,
              UserIdentityId = userIdentityId,
              AddedAt = comment.AddedAt,
              UpdatedAt = DateTime.Now
            };
            await _commentService.Update(commentUpdateDto,commentId);
            var startIndex = TempData["startIndex"];

            return Redirect($"Index?startIndex={startIndex}&projectId={projectId}");
        }
        [RoleAuthorize(["Manager"])]
        [HttpPost]
        public async Task<IActionResult> AddManager(ProjectPageModel projectPageModel)
        {
            UserIdentity? managerIdentity = await _authService.GetUserByEmail(projectPageModel.ManagerName);
            bool managerExists = await _managerService.Get(x => x.UserIdentityId == managerIdentity.Id) != null;

            if (managerExists)
            {
                var managerIdentities = GetString<UserIdentity>("ManagerIdentities");

                managerIdentities.Add(managerIdentity);

                SetString("ManagerIdentities", managerIdentities);


            }
            else
            {
                TempData["error"] = "Yönetici bulunamadı";
            }

            return RedirectToAction("Add", projectPageModel);
        }
        [HttpPost]
        [RoleAuthorize(["Manager"])]
        public async Task<IActionResult> AddUser(ProjectPageModel projectPageModel)
        {
            UserIdentity? userIdentity = await _authService.GetUserByEmail(projectPageModel.UserName);
            if (userIdentity != null)
            {

                var userIdentities = GetString<UserIdentity>("UserIdentities");
                userIdentities.Add(userIdentity);
                SetString("UserIdentities", userIdentities);


            }
            return RedirectToAction("Add", projectPageModel);



        }
        [RoleAuthorize(["Manager"])]
        [HttpPost]
        public IActionResult RemoveManager(ProjectPageModel projectPageModel)
        {
            var managerIdentities = GetString<UserIdentity>("ManagerIdentities");

            var manager = managerIdentities!.Last();
            managerIdentities.Remove(manager);
            HttpContext.Session.SetString("ManagerIdentities", JsonSerializer.Serialize(managerIdentities));
            return RedirectToAction("Add",projectPageModel);

        }
        [RoleAuthorize(["Manager"])]
        [HttpPost]
        public IActionResult RemoveUser(ProjectPageModel projectPageModel)
        {
            var userIdentities = GetString<UserIdentity>("UserIdentities");

            var user = userIdentities!.Last();
            userIdentities.Remove(user);
            SetString("UserIdentities",userIdentities);
            return RedirectToAction("Add", projectPageModel);

        }

        [RoleAuthorize(["Manager"])]
        public async Task<IActionResult> Add(ProjectPageModel projectPageModel)
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            var managerIdentities = GetString<UserIdentity>("ManagerIdentities");
            var userIdentities = GetString<UserIdentity>("UserIdentities");
            projectPageModel.ManagerIdentities = managerIdentities;
            projectPageModel.UserIdentities = userIdentities;

            return View(projectPageModel);
        }
        [RoleAuthorize(["Manager"])]
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectPageModel projectPageModel)
        
       {
            var managerIdentityId = await _authService.GetUserIdentityId();
            var managerIdentity = await _managerService.Get(x => x.UserIdentityId == managerIdentityId);

            var managerIdentities = GetString<UserIdentity>("ManagerIdentities");
            var userIdentities = GetString<UserIdentity>("UserIdentities");

            List<Guid>? managerIds = managerIdentities.Select(x => x.Id).Distinct().ToList();
            List<Manager> managers = _managerService.Where(x => managerIds.Contains(x.UserIdentityId));
            List<Guid> userIds = userIdentities.Select(x => x.Id).Distinct().ToList();
            List<User> users = _userService.Where(x => userIds.Contains(x.UserIdentityId));
            Guid projectId = Guid.NewGuid();
            ProjectDto projectDto = new ProjectDto()
            {
                Id = projectId,
                AddedAt = DateTime.Now,
                Description = projectPageModel.Description,
                Name = projectPageModel.Name,
                Status = projectPageModel.Status,
                Version = projectPageModel.Version,
                ManagerId = managerIdentity.Id
            };
           
            await _projectService.Add(projectDto);
            foreach (var manager in managers)
            {
                ProjectManager projectManager = new ProjectManager()
                {
                    ManagerId = manager.Id,
                    ProjectId = projectId,
                };
               await _projectManagerService.Add(projectManager);
            }
            string url = $"/Project/Index/{projectId}";
            foreach (var user in users)
            {
                ProjectUser projectUser = new ProjectUser()
                {
                    UserId = user.Id,
                    ProjectId = projectId,
                };

                _cacheService.SetClass("messages", user.UserIdentityId, async () => await _messageService.Filter(0, x => (DateTime)x.AddedAt, x => x.ReceiverId == user.UserIdentityId), TimeSpan.FromHours(1), TimeSpan.FromMinutes(20));

                await _notificationService.Notify(user.UserIdentityId);
                await _projectUserService.Add(projectUser);
            }
            

            return Redirect("/Project/Add");

        }
        private void SetString<T>(string key, List<T> value) where T : class
        {
            HttpContext.Session.SetString(key, JsonSerializer.Serialize(value));

        }
        private List<T> GetString<T>(string key) where T: class
        {
            var json = HttpContext.Session.GetString(key);
            var list = string.IsNullOrEmpty(json) ? new List<T>() : JsonSerializer.Deserialize<List<T>>(json);
            return list;
        }
    }
}
