using Auth.Services;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystemMVC.Models
{
    public class JobPageModel 
    {
        private readonly IService<Project, ProjectDto, ProjectUpdateDto> _projectService;
        private readonly AuthService _authService;
        public JobPageModel()
        {
        }
        public JobPageModel(IService<Project, ProjectDto, ProjectUpdateDto> projectService, AuthService authService)
        {
            _projectService = projectService;
            _authService = authService;
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string Title { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string Description { get; set; }
        public string Status { get; set; } = "Devam ediyor";
        public TimeSpan Time { get; set; }
        public int? UserId { get; set; }
        public int? ManagerId { get; set; }

        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public string ProjectName { get; set; }
        public Guid? FileUploadId { get; set; }
        public string? FileName { get; set; }
        public Guid? UserIdentityId { get; set; }
        public List<string> StatusOptions = new List<string>()
        {
            "Devam ediyor", "Beklemede", "Tamamlandı", "İptal Edildi"
        };
        public DateTime? AddedAt { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "(Boş Bırakılamaz)")]
        [EmailAddress(ErrorMessage = "(Email gerekli)")]
        public string UserName { get; set; }
    }
}
