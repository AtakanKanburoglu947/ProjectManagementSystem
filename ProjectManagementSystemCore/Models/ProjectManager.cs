﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class ProjectManager
    {
        public Guid ProjectId { get; set; }
        public int ManagerId { get; set; }
    }
}
