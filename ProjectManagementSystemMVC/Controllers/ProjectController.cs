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
    public class ProjectController : Controller
    {
        private readonly AuthService _authService;
        private readonly FileService _fileService;
        private readonly IService<Project,ProjectDto,ProjectUpdateDto> _projectService;
        private readonly IService<ProjectUser, ProjectUser, ProjectUser> _projectUserService;
        private readonly IService<ProjectManager, ProjectManager, ProjectManager> _projectManagerService;
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly IService<User, UserDto, UserUpdateDto> _userService;
        private readonly IService<Comment,CommentDto, CommentUpdateDto> _commentService;
        public ProjectController(AuthService authService,IService<Project, ProjectDto, ProjectUpdateDto> projectService
        , IService<ProjectUser, ProjectUser, ProjectUser> projectUserService, IService<User, UserDto, UserUpdateDto> userService
            , IService<ProjectManager, ProjectManager, ProjectManager> projectManagerService, IService<Manager, ManagerDto, ManagerUpdateDto> managerService, 
            IService<Comment, CommentDto, CommentUpdateDto> commentService, FileService fileService)
        {
            _authService = authService;
            _projectService = projectService;
            _projectUserService = projectUserService;
            _userService = userService;
            _projectManagerService = projectManagerService;
            _managerService = managerService;
            _commentService = commentService;
            _fileService = fileService;
        }
        [ServiceFilter(typeof(ProjectAccessFilter))]
        public async Task<IActionResult> Index(Guid id)
        {
            Project project = await _projectService.Get(id);      
            List<ProjectUser> projectUsers = _projectUserService.Where(x=> x.ProjectId == project.Id); 
            List<ProjectManager> projectManagers = _projectManagerService.Where(x=>x.ProjectId == project.Id);
            List<int> managerIds = projectManagers.Select(x=>x.ManagerId).Distinct().ToList();
            List<int> userIds = projectUsers.Select(x=>x.UserId).Distinct().ToList();
            List<Manager> managers = _managerService.Where(x => managerIds.Contains(x.Id));
            List<User> users = _userService.Where(x => userIds.Contains(x.Id));
            List<Guid> userIdentityIds = users.Select(x=>x.UserIdentityId).Distinct().ToList();
            List<Guid> managerIdentityIds = managers.Select(x => x.UserIdentityId).Distinct().ToList();
            List<UserIdentity> managerIdentities = new List<UserIdentity>();

            List<Comment> comments = _commentService.Where(x => x.ProjectId == project.Id);

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
                UserIdentityId = await _authService.GetUserIdentityId()
                
            };
            if (userIdentityIds.Count > 0)
            {
                List<UserIdentity> userIdentities = new List<UserIdentity>();
                foreach (var userIdentityId in userIdentityIds)
                {
                    userIdentities.Add(await _authService.GetUserById(userIdentityId));
                }
                projectPageModel.UserIdentities = userIdentities;
                
            }
            if (comments.Count > 0)
            {
               
                List<CommentDetails> commentDetails = new List<CommentDetails>();
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
                        
                    };
                    if (comment.FileUploadId != null)
                    {
                        var commentFile = await _fileService.GetFile((Guid)comment.FileUploadId);
                        commentDetail.FileId = commentFile.Id;
                        commentDetail.FileName = commentFile.Name;
                    }
                    commentDetails.Add(commentDetail);
                }
                projectPageModel.CommentDetails = commentDetails;
            }
            return View(projectPageModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid projectId, CommentDetails newComment, IFormFile file)
        {
            if (string.IsNullOrEmpty(newComment.Text))
            {
                return Redirect($"/Project/Index/{projectId}");

            }
            var commentDto = new CommentDto()
            {
                ProjectId = projectId,
                Text = newComment.Text,
                UserIdentityId = await _authService.GetUserIdentityId(),

            };
            if (file != null && file.Length >0) {
                Guid fileUploadId = await _fileService.Upload(file, [".txt"], await _authService.GetUserIdentityId(),null);
                commentDto.FileUploadId = fileUploadId;
            }
            await _commentService.Add(commentDto);
            return Redirect($"/Project/Index/{projectId}");

        }
        public async Task<IActionResult> RemoveComment(Guid commentId, Guid projectId)
        {
            var comment = await _commentService.Get(commentId);
            await _commentService.Remove(x => x.Id == commentId);
            if (comment.FileUploadId != null || comment.FileUploadId != Guid.Empty)
            {
                await _fileService.RemoveFile(comment.FileUploadId);
            }

            return Redirect($"/Project/Index/{projectId}");
        }
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentDto commentDto,Guid commentId, Guid projectId, Guid userIdentityId)
        {
            var comment = await _commentService.Get(commentId);
            var commentUpdateDto = new CommentUpdateDto() {
              Id = commentId,
              ProjectId =  projectId,
              Text = commentDto.Text,
              UserIdentityId = userIdentityId
            };
            await _commentService.Update(commentUpdateDto,commentId);
            return Redirect($"/Project/Index/{projectId}");

        }
    }
}
