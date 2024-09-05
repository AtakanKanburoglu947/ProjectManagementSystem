using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public List<Manager> Managers { get; } = [];
        public List<ProjectManager> ProjectManagers { get; } = [];
        public List<User> Users { get; } = [];
        public List<ProjectUser> ProjectUsers { get; } = [];
        public FileUpload? FileUpload { get; set; }
        public Guid? FileUploadId { get; set; }

    }
}
