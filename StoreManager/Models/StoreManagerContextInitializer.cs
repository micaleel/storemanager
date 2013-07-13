using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreManager.Models
{
    public class StoreManagerContextInitializer : DropCreateDatabaseAlways<StoreManagerContext>
    {
        public static IEnumerable<Item> CreateItems(List<ItemStatus> itemStatuses, int count = 10)
        {
            var randomizer = new Random();

            for (var i = 0; i < count; i++)
            {
                yield return new Item
                     {
                         Name = string.Format("Item {0}", i),
                         Status = itemStatuses[randomizer.Next(itemStatuses.Count)]
                     };
            }
        }

        public static IEnumerable<Location> CreateLocations(int count = 10)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new Location
                {
                    Name = string.Format("Location {0}", i),
                };
            }
        }

        public static IEnumerable<Movement> CreateMovements(List<Item> items, List<Location> locations, int count = 10)
        {
            var randomDates = new RandomDates();
            var randomizer = new Random();

            for (var i = 0; i < count; i++)
            {
                var item = items[randomizer.Next(items.Count)];
                var location = locations[randomizer.Next(locations.Count)];

                yield return new Movement
                {
                    Notes = string.Format("Remarks for moving item '{0}' to location '{1}'", item.Name, location.Name),
                    ItemId =  item.Id, LocationId = location.Id,
                    Date =  randomDates.Date(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow)
                };
            }
        }
        public static List<ItemStatus> CreateItemStatuses()
        {
            return new List<ItemStatus>
                {
                    new ItemStatus {Name = "Damaged"},
                    new ItemStatus {Name = "Fair"},
                    new ItemStatus {Name = "Stolen"},
                    new ItemStatus {Name = "Good"}
                };
        }
        protected override void Seed(StoreManagerContext context)
        {
            var itemStatuses = CreateItemStatuses().ToList();

            itemStatuses.ForEach(itemStatus => context.ItemStatuses.Add(itemStatus));

            var items = CreateItems(itemStatuses, 50).ToList();
            items.ForEach(item => context.Items.Add(item));
            context.SaveChanges();

            var locations = CreateLocations(25).ToList();
            locations.ForEach(location => context.Locations.Add(location));
            context.SaveChanges();
            
            var movements = CreateMovements(items.ToList(), locations.ToList(), 200).ToList();
            movements.ForEach(itemStatus => context.Movements.Add(itemStatus));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}