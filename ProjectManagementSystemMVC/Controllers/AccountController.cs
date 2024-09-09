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
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly FileService _fileService;
        private readonly IService<Comment, CommentDto, CommentUpdateDto> _commentService;
        private readonly IService<Project,ProjectDto,ProjectUpdateDto> _projectService;
        public AccountController(AuthService authService,FileService fileService, IService<Comment, CommentDto, CommentUpdateDto> commentService,
            IService<Project,ProjectDto,ProjectUpdateDto> projectService)
        {
            _authService = authService;
            _fileService = fileService;
            _commentService = commentService;
            _projectService = projectService;

        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            AccountPageModel accountPageModel = new AccountPageModel()
            {
                Email = userIdentity.Email,
                UserName = userIdentity.UserName,

            };
            List<FileUpload>? files =  await _fileService.GetFilesOfUser(userIdentityId)!;
            if (files != null)
            {
                accountPageModel.Files = files;   
            }
            List<Comment>? comments = _commentService.Where(x=>x.UserIdentityId == userIdentityId);
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
                        UserName =  userIdentity.UserName
                    };
                    if (comment.FileUploadId != null)
                    {
                        var commentFile = await _fileService.GetFile((Guid)comment.FileUploadId);
                        commentDetail.FileId = commentFile.Id;
                        commentDetail.FileName = commentFile.Name;
                    }
                    commentDetails.Add(commentDetail);
                }
                accountPageModel.Comments = commentDetails;
            }
            return View(accountPageModel);
        }
        public async Task<IActionResult> Download(Guid id)
        {
            FileUpload? file = await _fileService.GetFile(id);
            return File(file.Data, "application/octet-stream", fileDownloadName: file.Name);
        }
    }
}
