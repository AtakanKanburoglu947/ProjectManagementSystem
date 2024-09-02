using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class PasswordDto
    {
        public string OldPassword { get; set; }
        [Password]
        public string NewPassword { get; set; }
    }
}
