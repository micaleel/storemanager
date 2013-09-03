using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using StoreManager.Models;

namespace StoreManager.Views.Stock {

    public class CreateStockModel : Models.Stock, IValidatableObject {

        public CreateStockModel() {
            Quantity = 1;
        }

        public int Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (Quantity < 1) {
                yield return new ValidationResult("Quantity cannot be less than or equal to 0");
            }
        }

        public static CreateStockModel Create(Item item) {
            return new CreateStockModel { ItemId = item.Id, Item = item };
        }

        public static IEnumerable<Models.Stock> Explode(CreateStockModel stock) {
            stock.BatchId = Guid.NewGuid();

            for (var i = 0; i < stock.Quantity; i++) {
                var cloned = Mapper.Map<CreateStockModel, Models.Stock>(stock);

                cloned.IsParent = i == 0;

                yield return cloned;
            }
        }
    }
}