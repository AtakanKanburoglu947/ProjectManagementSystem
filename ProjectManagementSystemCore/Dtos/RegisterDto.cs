using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore.Dtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        [Password]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
