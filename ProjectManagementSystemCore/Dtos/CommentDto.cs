﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class CommentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }

    }
}
