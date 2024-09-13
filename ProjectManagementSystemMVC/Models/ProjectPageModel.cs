using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemMVC.Models
{
    public class ProjectPageModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public List<UserIdentity>? UserIdentities { get; set; }
        public List<UserIdentity> ManagerIdentities { get; set; }
        public List<CommentDetails>? CommentDetails { get; set; }
        public CommentDto NewComment { get; set; }
        public Guid? UserIdentityId { get; set; }
        public DateTime? AddedAt { get; set; }
        public string ManagerName { get; set; }
        public string UserName { get; set; }

    }
}
