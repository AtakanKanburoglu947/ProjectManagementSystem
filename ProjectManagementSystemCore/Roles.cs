using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore
{
    public static class Roles
    {
        public readonly static Role User = new() { Description = "User", Id = 1, Title = "User" } ;
        public readonly static Role Manager = new() { Description = "Manager", Id = 2, Title = "Manager" };
        public readonly static Role Admin = new() { Description = "Admin", Id = 3, Title = "Admin" };
    }
}
