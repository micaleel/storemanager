using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    }
}