using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid? FileUploadId { get; set; }
        public FileUpload FileUpload { get; set; }
        public Guid? UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
        public DateTime? AddedAt { get; set; }

    }
}
