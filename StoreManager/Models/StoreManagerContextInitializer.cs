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

        public static IEnumerable<Location> CreateLocations() {
            var locationNames = new[]
                {
                    "JSS 1A",
                    "JSS 1B",
                    "JSS 1C",
                    "JSS 1D",
                    "JSS 1E",
                    "Staff Toilet", "Student Toilet", "Home Economics Laboratory",
                    "Integrated Science Laboratory",
                    "Basic Technology Laboratory",
                    "Creative Arts Studio",
                    "Cafeteria",
                    "Kitchen",
                    "Director's Office","Principal's Office",
                    "Staff Room 1", "Staff Room 2", "School Clinic", "School Mosque", "TOEFL Room", "E-Library", "Server Room", "Store Room", "Matron's Office", "Finantil Admin Office", "Chairman's Office", "Reception Area", "School Hall", "Student Seating Area", "School Courtyard"
                };

            for (var i = 0; i < locationNames.Count(); i++) {
                yield return new Location {
                    Name = locationNames[i]
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

            base.Seed(context);
        }
    }
}