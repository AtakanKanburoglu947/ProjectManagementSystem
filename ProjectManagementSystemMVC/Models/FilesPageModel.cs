using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemMVC.Models
{
    public class FilesPageModel
    {
        public int Count { get; set; }
        public List<FileUpload> Files { get; set; }
    }
}
