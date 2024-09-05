using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public Guid UserIdentityId { get; set; }
        public UserIdentity UserIdentity { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<Job>? Jobs { get; set; } = [];
        public List<Comment>? Comments { get; } = [];
        public List<Project> Projects { get; } = [];
        public List<ProjectManager> ProjectManagers { get; } = [];
    }
}
