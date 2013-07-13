using System.Data.Entity;

namespace StoreManager.Models
{
    public class StoreManagerContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemStatus> ItemStatuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Movement> Movements { get; set; }
    }
}