using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class MessageDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}
