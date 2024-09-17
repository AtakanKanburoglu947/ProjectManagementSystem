using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystemMVC.Models
{
    public class ProjectPageModel
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string Description { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string Version { get; set; } = "1.0";
        public string Status { get; set; } = "Devam ediyor";
        public List<UserIdentity>? UserIdentities { get; set; }
        public List<UserIdentity>? ManagerIdentities { get; set; }
        public List<CommentDetails>? CommentDetails { get; set; }
        public Guid? UserIdentityId { get; set; }
        public DateTime? AddedAt { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        [EmailAddress(ErrorMessage = "(Email gerekli)")]

        public string ManagerName { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        [EmailAddress(ErrorMessage = "(Email gerekli)")]
        public string UserName { get; set; }

        public string? ManagerError { get; set; }
        public string? UserError { get; set; }
        public List<string> StatusOptions = new List<string>()
        {
            "Devam ediyor", "Beklemede"
        };
    }
}
