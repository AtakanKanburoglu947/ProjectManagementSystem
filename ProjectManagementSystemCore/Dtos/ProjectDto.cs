﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManagerId { get; set; }
    }
}
