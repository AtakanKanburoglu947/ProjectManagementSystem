using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
        public Guid? FileUploadId { get; set; }
        public FileUpload? FileUpload { get; set; }
    }
}
