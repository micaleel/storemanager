using System.Data.Entity;

namespace StoreManager.Models {
    public class StoreManagerContext : DbContext {
        public DbSet<Item> Items { get; set; }
        public DbSet<StockCondition> StockConditions { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder) {
            
            //builder.Entity<Location>().HasMany(x=>x.Movements).WithRequired(x=>x.Location).WillCascadeOnDelete(false);
            builder.Entity<Movement>().HasRequired(x=>x.Location).WithMany(x=>x.Movements).WillCascadeOnDelete(false);
            builder.Entity<Movement>().HasRequired(x => x.Location).WithMany(x => x.Movements).WillCascadeOnDelete(false);

            base.OnModelCreating(builder);
        }
    }
}