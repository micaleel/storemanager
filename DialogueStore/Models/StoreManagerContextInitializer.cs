using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DialogueStore.Repositories;

namespace DialogueStore.Models {
    public class DialogueStoreContextInitializer : DropCreateDatabaseAlways<DialogueStoreContext> {

        public static IEnumerable<Item> CreateItems(List<StockCondition> stockConditions) {
            var itemNames = new[] { "Desktop Computer", "Laptop Computer", "Smart Board", "Microwave" };

            for (var i = 0; i < itemNames.Count(); i++) {
                yield return new Item {
                    Name = itemNames[i],
                    Discontinued = false,
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
        protected override void Seed(DialogueStoreContext context) {

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

            var stockConditionRepo = new StockConditionRepository(context);
            var stockConditions = CreateStockConditions().ToList();
            stockConditions.ForEach(stockCondition => stockConditionRepo.Add(stockCondition));

            var locationRepo = new LocationRepository(context);
            var locations = CreateLocations().ToList();
            locations.ForEach(location => locationRepo.Add(location));

            var itemRepo = new ItemRepository(context);
            var items = CreateItems(stockConditions).ToList();
            items.ForEach(item => itemRepo.Add(item));

            var random = new Random();

            // Add stock
            foreach (var item in items) {
                var stocks = new List<Stock>();
                var batchId = Guid.NewGuid();

                for (var i = 0; i < random.Next(1, 5); i++) {
                    stocks.Add(new Stock {
                        BatchId = batchId,
                        BatchPrice = random.Next(10, 2000),
                        UnitPrice = random.Next(10, 2000),
                        ItemId = item.Id, 
                        StockConditionId = stockConditions[random.Next(0, stockConditions.Count - 1)].Id
                    });
                }

                itemRepo.AddStocks(item.Id, stocks);
            }

            context.SaveChanges();
            base.Seed(context);
        }
    }
}