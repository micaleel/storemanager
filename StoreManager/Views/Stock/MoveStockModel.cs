using System.ComponentModel.DataAnnotations;

namespace StoreManager.Views.Stock {

    public class MoveStockModel  {

        public int StockId { get; set; }

        public Models.Stock Stock { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public void SetStock(Models.Stock stock) {
            Stock = stock;
            StockId = stock.Id;
        }
    }
}