using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class UserDto
    {
        public Guid UserIdentityId { get; set; }
        public int RoleId { get; set; }
    }
}
