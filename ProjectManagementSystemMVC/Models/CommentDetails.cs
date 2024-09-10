namespace ProjectManagementSystemMVC.Models
{
    public class CommentDetails
    {
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public Guid CommentId { get; set; }
        public  Guid UserIdentityId { get; set; }
        public Guid? FileId { get; set; }
        public string? FileName { get; set; }
    }
}
