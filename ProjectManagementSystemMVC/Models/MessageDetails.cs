namespace ProjectManagementSystemMVC.Models
{
    public class MessageDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime? AddedAt { get; set; }

    }
}
