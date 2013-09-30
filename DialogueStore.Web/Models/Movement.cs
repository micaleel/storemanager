using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DialogueStore.Web.Models
{

    public class Movement
    {
        public int Id { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public int StockId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }

        [ForeignKey("FromLocationId")]
        public virtual Location FromLocation { get; set; }

        [Display(Name = "From Location")]
        public int? FromLocationId { get; set; }

        [Display(Name = "To Location")]
        public int ToLocationId { get; set; }

        [ForeignKey("ToLocationId")]
        public virtual Location ToLocation { get; set; }

        [Display(Name = "Requisition Note")]
        public string RequisitionDoc { get; set; }

        [Display(Name = "Authorization Note")]
        public string AuthorizationDoc { get; set; }

        public override string ToString()
        {
            var toLocation = (ToLocation != null) ? ToLocation.Name : string.Empty;

            if (FromLocationId.HasValue && FromLocation != null)
            {
                return string.Format("Stock item {0} from {1} to {2}",
                    Stock.Item.Name, FromLocation.Name, toLocation);
            }

            return string.Format("Stock item {0} to {1}",
                Stock.Item.Name, toLocation);
        }
    }
}