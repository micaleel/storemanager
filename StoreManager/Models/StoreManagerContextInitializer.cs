using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StoreManager.Models {
    public class StoreManagerContextInitializer : DropCreateDatabaseAlways<StoreManagerContext> {

        public static IEnumerable<Item> CreateItems(List<StockCondition> itemConditions) {
            var itemNames = new[] { "Desktop Computer", "Laptop Computer", "Smart Board", "Microwave" };

            for (var i = 0; i < itemNames.Count(); i++) {
                yield return new Item {
                    Name = itemNames[i],
                    Discontinued = false,
                    InternalId = Guid.NewGuid(),
                    ReorderLevel = 2
                };
            }
        }

        public static IEnumerable<Location> CreateLocations(int count = 10) {
            for (var i = 0; i < count; i++) {
                yield return new Location {
                    Name = string.Format("Location {0}", i),
                };
            }
        }

        public static IEnumerable<Movement> CreateMovements(List<Item> items, List<Location> locations, int count = 10) {
            var randomDates = new RandomDates();
            var randomizer = new Random();

            for (var i = 0; i < count; i++) {
                var item = items[randomizer.Next(items.Count)];
                var location = locations[randomizer.Next(locations.Count)];

                yield return new Movement {
                    Notes = string.Format("Moved '{0}' to '{1}'", item.Name, location.Name),
                    ItemId = item.Id,
                    LocationId = location.Id,
                    Date = randomDates.Date(DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow)
                };
            }
        }
        public static List<StockCondition> CreateStockConditions() {
            return new List<StockCondition>
                {
                    new StockCondition {Name = "Damaged"},
                    new StockCondition {Name = "Fair"},
                    new StockCondition {Name = "Stolen"},
                    new StockCondition {Name = "Good"}
                };
        }
        protected override void Seed(StoreManagerContext context) {

            var user = new User {
                LastLogin = DateTime.UtcNow,
                Locked = false,
                Approved = true,
                Role = Role.Admin.ToString(),
                DisplayName = "Administrator",
                Email = "admin@storemanager.com",
                Password = "password",
                Phone = "08093188347",
                SecretAnswer = "What is Dialogue?"
            };

            user.SecretAnswer = "Dialogue";
            user.UserName = "admin";
            context.Users.Add(user);
            context.SaveChanges();

            var itemConditions = CreateStockConditions().ToList();

            itemConditions.ForEach(itemCondition => context.StockConditions.Add(itemCondition));

            var locations = CreateLocations().ToList();
            locations.ForEach(location => context.Locations.Add(location));
            context.SaveChanges();

            var items = CreateItems(itemConditions).ToList();
            items.ForEach(item => context.Items.Add(item));
            context.SaveChanges();


            var movements = CreateMovements(items.ToList(), locations.ToList(), 200).ToList();
            movements.ForEach(itemCondition => context.Movements.Add(itemCondition));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}