using Auth.Services;
using ProjectManagementSystemCore;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;

namespace ProjectManagementSystemAPI
{
    public static class DatabaseConfiguration
    {
        public async static Task Seed(AppDbContext appDbContext, IConfiguration configuration) 
        {
            appDbContext.Database.EnsureCreated();
            if (appDbContext.UserIdentities.Any() || appDbContext.Roles.Any() || appDbContext.Managers.Any())
            {
                return;
            }
            string? managerPassword = configuration["Passwords:ManagerPassword"];
            string? adminPassword = configuration["Passwords:AdminPassword"];
            Guid managerId = Guid.NewGuid();
            Guid adminId = Guid.NewGuid();
            (string, string) hashedManagerPassword = PasswordService.HashPassword(managerPassword!);
            (string, string) hashedAdminPassword = PasswordService.HashPassword(adminPassword!);
            appDbContext.UserIdentities.AddRange(
                    new UserIdentity()
                    {
                        Id = managerId, Email = "manager@example.com",  UserName = "Manager",
                        PasswordHash = hashedManagerPassword.Item1, PasswordSalt = hashedManagerPassword.Item2
                    },
                    new UserIdentity()
                    {
                        Email = "admin@example.com", Id = adminId, 
                        UserName = "Admin", PasswordHash = hashedAdminPassword.Item1, PasswordSalt= hashedAdminPassword.Item2
                    }
                );
            appDbContext.Roles.AddRange(
                    new Role() { Title = Roles.User.Title, Description = Roles.User.Description},
                    new Role() { Title = Roles.Manager.Title, Description = Roles.Manager.Description },
                    new Role() { Title = Roles.Admin.Title, Description = Roles.Admin.Description }
                );
            appDbContext.Managers.Add(
                    new Manager() { RoleId = Roles.Manager.Id, UserIdentityId = managerId }
                );
            appDbContext.Users.Add(new User() { RoleId = Roles.Admin.Id, UserIdentityId = adminId });
            await appDbContext.SaveChangesAsync();
        }
    }
}
