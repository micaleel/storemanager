using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DialogueStore.Models {

    public class Stock {

        public Stock() {
            DateAdded = DateTime.UtcNow;
            BatchId = Guid.Empty;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [Display(Name = "Batch #")]
        public Guid BatchId { get; set; }

        [Display(Name = "Ref #")]
        public string RefNo { get; set; }

        [Display(Name = "Condition")]
        public int StockConditionId { get; set; }

        [ForeignKey("StockConditionId")]
        public virtual StockCondition Condition { get; set; }

        [Display(Name = "Is Expired?")]
        public bool IsExpired {
            get { return ExpiryDate <= DateTime.UtcNow; }
        }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}", NullDisplayText = "")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}", NullDisplayText = "")]
        public DateTime? PurchaseDate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Purchase Price (Batch)")]
        public decimal? BatchPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Purchase Price (Unit)")]
        public decimal? UnitPrice { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, NullDisplayText = "", DataFormatString = "{0:dd/mm/yy}")]
        public DateTime DateAdded { get; set; }

        public bool IsParent { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public virtual List<Movement> Movements { get; set; }
    }
}