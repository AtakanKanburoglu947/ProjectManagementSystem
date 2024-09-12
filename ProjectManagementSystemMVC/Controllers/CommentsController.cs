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
    public class CommentsController : Controller
    {
        private readonly AuthService _authService;

        private readonly FileService _fileService;
        private readonly IService<Comment, CommentDto, CommentUpdateDto> _commentService;
        private readonly IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        private readonly NotificationService _notificationService;

        public CommentsController(AuthService authService, FileService fileService, IService<Comment, CommentDto, CommentUpdateDto> commentService,
            IService<Project, ProjectDto, ProjectUpdateDto> projectService, NotificationService notificationService)
        {
            _authService = authService;
            _fileService = fileService;
            _commentService = commentService;
            _projectService = projectService;
            _notificationService = notificationService;
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

            int count = _commentService.Count(x=>x.UserIdentityId == userIdentityId);
            Expression<Func<Comment, DateTime>> expression = x => (DateTime)x.AddedAt;
            List<Comment>? comments = await _commentService.Filter(id,expression, x => x.UserIdentityId == userIdentityId);
            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            PaginationModel<CommentDetails, NoData> paginationModel = new PaginationModel<CommentDetails, NoData>()
            {
                Pagination = new PaginationViewModel() { Count = count }
            };
            List<CommentDetails> commentDetails = new List<CommentDetails>();

            if (comments != null)
            {
                foreach (Comment comment in comments)
                {
                    Guid projectId = comment.ProjectId;
                    Project project = await _projectService.Get(projectId);
                    CommentDetails commentDetail = new CommentDetails()
                    {
                        CommentId = comment.Id,
                        Text = comment.Text,
                        ProjectId = projectId,
                        ProjectName = project.Name,
                        UserName = userIdentity.UserName,
                        AddedAt = comment.AddedAt,
                        
                    };
                    if (comment.UpdatedAt != null)
                    {
                        commentDetail.UpdatedAt = comment.UpdatedAt;
                    }
                    if (comment.FileUploadId != null)
                    {
                        var commentFile = await _fileService.GetFile((Guid)comment.FileUploadId);
                        commentDetail.FileId = commentFile.Id;
                        commentDetail.FileName = commentFile.Name;
                    }
                    commentDetails.Add(commentDetail);
                }
                paginationModel.Dataset = commentDetails;
                
            }
            return View(paginationModel);
        }
        public async Task<IActionResult> Remove()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            List<Comment> comments = _commentService.Where(x=>x.UserIdentityId == userIdentityId);
            if (comments.Any()) {
                foreach (var item in comments)
                {
                    if (item.FileUploadId != null)
                    {
                        await _fileService.RemoveFile(item.FileUploadId);
                    }
                    await _commentService.Remove(item.Id);
                }
            }
            return RedirectToAction("Index");

        }
    }
}
