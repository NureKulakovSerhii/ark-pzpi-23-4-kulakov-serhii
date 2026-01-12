using Data.Configurations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<ModerationTask> ModerationTasks { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserFavoriteAdvert> UserFavoriteAdverts { get; set; }
        public DbSet<WarehouseDevice> WarehouseDevices { get; set; }
        public DbSet<DeviceTelemetry> DeviceTelemetries { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvertConfiguration());
            modelBuilder.ApplyConfiguration(new ModerationTaskConfiguration());
            modelBuilder.ApplyConfiguration(new SupportTicketConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserFavoriteAdvertConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseDeviceConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceTelemetryConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
