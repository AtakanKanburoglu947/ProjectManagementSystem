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
    public class CommentsController : Controller
    {
        private readonly AuthService _authService;

        private readonly FileService _fileService;
        private readonly IService<Comment, CommentDto, CommentUpdateDto> _commentService;
        private readonly IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        public CommentsController(AuthService authService, FileService fileService, IService<Comment, CommentDto, CommentUpdateDto> commentService,
            IService<Project, ProjectDto, ProjectUpdateDto> projectService)
        {
            _authService = authService;
            _fileService = fileService;
            _commentService = commentService;
            _projectService = projectService;

        }
        public async Task<IActionResult> Index(int id)
        {
            ViewData["id"] = id;
            Guid userIdentityId = await _authService.GetUserIdentityId();
            int count = _commentService.Count(x=>x.UserIdentityId == userIdentityId);
            if (id > 0)
            {
                id = id * 4;
            }
            List<Comment>? comments = await _commentService.Filter(id, x => x.UserIdentityId == userIdentityId);
            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            CommentPageModel commentPageModel = new CommentPageModel();
            commentPageModel.Count = count;
            List<CommentDetails> commentDetails = new List<CommentDetails>();

            if (comments != null)
            {
                foreach (Comment comment in comments.OrderBy(x=>x.AddedAt))
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
                commentPageModel.CommentDetails = commentDetails;   

            }
            return View(commentPageModel);
        }
    }
}
