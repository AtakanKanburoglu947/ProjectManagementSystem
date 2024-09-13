using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class ManagerUpdateDto
    {
        public int Id { get; set; }
        public Guid UserIdentityId { get; set; }
        public int RoleId { get; set; }
        public int Notifications { get; set; }

    }
}
