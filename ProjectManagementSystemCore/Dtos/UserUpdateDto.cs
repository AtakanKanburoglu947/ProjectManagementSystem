using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public Guid UserIdentityId { get; set; }
        public int RoleId { get; set; }
    }
}
