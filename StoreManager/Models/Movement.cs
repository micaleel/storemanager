using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models {

    public class Movement {
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
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        [Display(Name = "Requisition Note")]
        public string RequisitionDoc { get; set; }

        [Display(Name = "Authorization Note")]
        public string AuthorizationDoc { get; set; }
    }
}