using Auth.Services;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;

namespace ProjectManagementSystemAPI
{
    public static class DatabaseConfiguration
    {
        public async static Task Seed(AppDbContext appDbContext, IConfiguration configuration) 
        {
            appDbContext.Database.EnsureCreated();
            if (appDbContext.UserIdentities.Any()
                || appDbContext.Roles.Any() || appDbContext.Managers.Any()
                )
            {
                return;
            }
            string managerPassword = configuration["Passwords:ManagerPassword"]!;
            Guid managerId = Guid.NewGuid();
            (string,string) hashedManagerPassword = PasswordService.HashPassword(managerPassword);
            appDbContext.UserIdentities.Add(new UserIdentity() { Id = managerId, Email = "manager@example.com",
                UserName = "Manager", PasswordHash = hashedManagerPassword.Item1, PasswordSalt = hashedManagerPassword.Item2
            });
            appDbContext.Roles.Add(new Role() { Description = "Manager", Title = "Manager" });
            appDbContext.Managers.Add(new Manager() { RoleId = 1, UserIdentityId = managerId });
            await appDbContext.SaveChangesAsync();
        }
    }
}
