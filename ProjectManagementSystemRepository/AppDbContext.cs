using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemRepository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}
