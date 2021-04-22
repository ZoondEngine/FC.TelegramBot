using FC.TelegramBot.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FC.TelegramBot.Core.Database
{
    public class DatabaseContext : DbContext
    {
        private string ConnectionString;
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleHasPermission> RolePermissions { get; set; }
        public DbSet<OrderMenu> OrderMenus { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemModifier> OrderItemModifiers { get; set; }
        public DbSet<OrderItemVolume> OrderItemVolumes { get; set; }
        public DbSet<OrderHistoryElement> OrderHistory { get; set; }

        public DatabaseContext(string host, string user, string pass, string db)
        {
            ConnectionString = "";

            ConnectionString += $"server={host};";
            ConnectionString += $"UserId={user};";
            ConnectionString += $"Password={pass};";
            ConnectionString += $"database={db};";
        }

        protected override void OnConfiguring( DbContextOptionsBuilder builder )
            => builder.UseMySql( ConnectionString );
    }
}
