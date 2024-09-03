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
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        public ICollection<User>? Users { get; set; }    

    }
}
