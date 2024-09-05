using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserIdentityId { get; set; }
        public Guid? FileUploadId { get; set; }

    }
}
