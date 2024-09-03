using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class ProjectUser
    {
        public int UserId { get; set; }
        public Guid ProjectId { get; set; }

    }
}
