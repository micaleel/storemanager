using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreManager.Views.Stock {

    public class MoveStockModel : IValidatableObject {
        public int StockId { get; set; }

        public Models.Stock Stock { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public void SetStock(Models.Stock stock) {
            Stock = stock;
            StockId = stock.Id;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (Quantity > Stock.Quantity) {
                yield return new ValidationResult(
                    "Quantity to be moved is more than available stock quantity on hand");
            }
        }
    }
}