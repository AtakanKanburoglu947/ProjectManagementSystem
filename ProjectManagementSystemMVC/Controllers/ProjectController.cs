using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;
using System.Linq.Expressions;

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
        public async Task<IActionResult> Index(Guid projectId,int startIndex)
        {
            ViewData["startIndex"] = startIndex;
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
            var commentDto = new CommentDto()
            {
                ProjectId = projectId,
                Text = newComment.Text,
                UserIdentityId = await _authService.GetUserIdentityId(),
                AddedAt = DateTime.Now

            };
            if (file != null && file.Length >0) {
                Guid fileUploadId = await _fileService.Upload(file, [".txt"], await _authService.GetUserIdentityId(),null);
                commentDto.FileUploadId = fileUploadId;
            }
            await _commentService.Add(commentDto);
            var startIndex = TempData["startIndex"];
            return Redirect($"Index?startIndex={startIndex}&projectId={projectId}");

        }
        public async Task<IActionResult> RemoveComment(Guid commentId, Guid projectId)
        {
            var comment = await _commentService.Get(commentId);
            await _commentService.Remove(x => x.Id == commentId);
            if (comment.FileUploadId != null || comment.FileUploadId != Guid.Empty)
            {
                await _fileService.RemoveFile(comment.FileUploadId);
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
    }
}
