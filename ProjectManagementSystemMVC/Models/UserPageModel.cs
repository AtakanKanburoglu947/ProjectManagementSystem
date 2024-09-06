using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemMVC.Models
{
    public class UserPageModel
    {
        public string UserName { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
