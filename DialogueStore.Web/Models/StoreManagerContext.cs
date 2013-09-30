using System.Data.Entity;

namespace DialogueStore.Web.Models {
    public class DialogueStoreContext : DbContext {
        public DbSet<Item> Items { get; set; }
        public DbSet<StockCondition> StockConditions { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<TimelineActivity> TimelineActivities { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder) {

            builder.Entity<Movement>()
                .HasRequired(x => x.ToLocation)
                .WithMany(x => x.ToMovements)
                .HasForeignKey(x=>x.ToLocationId)
                .WillCascadeOnDelete(false);

            builder.Entity<Movement>()
                .HasOptional(x => x.FromLocation)
                .WithMany(x => x.FromMovements)
                .HasForeignKey(x=>x.FromLocationId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(builder);
        }
    }
}