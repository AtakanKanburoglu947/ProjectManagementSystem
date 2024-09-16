using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class JobUpdateDto
    {
        public Guid Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? FileUploadId { get; set; }
        public Guid? UserIdentityId { get; set; }
        public DateTime? AddedAt { get; set; }

    }
}
