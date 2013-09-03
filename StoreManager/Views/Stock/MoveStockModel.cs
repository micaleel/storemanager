using System.ComponentModel.DataAnnotations;

namespace StoreManager.Views.Stock {

    public class MoveStockModel {

        public int StockId { get; set; }

        public Models.Stock Stock { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Requisition Note")]
        public string RequisitionDoc { get; set; }

        [Display(Name = "Authorization Note")]
        public string AuthorizationDoc { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public static MoveStockModel Create(Models.Stock stock) {
            return new MoveStockModel { Stock = stock, StockId = stock.Id };
        }
    }
}