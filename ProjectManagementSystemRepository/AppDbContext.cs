using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Models;

namespace ProjectManagementSystemRepository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasMany(x => x.Managers).WithMany(x => x.Projects).UsingEntity<ProjectManager>();
            modelBuilder.Entity<Project>().HasMany(x => x.Users).WithMany(x => x.Projects).UsingEntity<ProjectUser>();
            modelBuilder.Entity<Message>().HasOne(x => x.Sender).WithMany().HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>().HasOne(x => x.Receiver).WithMany().HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Job>().HasOne(x=>x.Manager).WithMany().HasForeignKey(x=>x.ManagerId).OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
