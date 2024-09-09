using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemMVC.Models
{
    public class AccountPageModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<FileUpload>? Files { get; set; }
        public List<CommentDetails>? Comments { get; set; }
    }
}
