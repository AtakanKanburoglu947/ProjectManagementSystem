using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemMVC.Models
{
    public class JobPageModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public TimeSpan Time {  get; set; }
        public int UserId { get; set; }
        public Guid ProjectId { get; set; }      
        public string ProjectName { get; set; }
        public Guid? FileUploadId { get; set; }
        public string? FileName { get; set; }
        public Guid? UserIdentityId { get; set; }
        public List<string> StatusOptions = new List<string>()
        {
            "Devam ediyor", "Beklemede", "Tamamlandı", "İptal Edildi"
        };
        public Guid? AddedAt { get; set; }
    }
}
