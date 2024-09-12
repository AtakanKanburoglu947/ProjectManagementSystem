using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid? SenderId { get; set; }
        public UserIdentity? Sender { get; set; }
        public Guid? ReceiverId { get; set; }
        public UserIdentity? Receiver { get; set; }

    }
}
