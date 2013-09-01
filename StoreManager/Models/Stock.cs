using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models {

    public class Stock : IValidatableObject {
        public Stock() {
            InternalId = Guid.Empty;
            Quantity = 1;
            DateAdded = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid InternalId { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [Display(Name = "Batch/Serial Number")]
        public string ReferenceId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Display(Name = "Condition")]
        public int StockConditionId { get; set; }

        [ForeignKey("StockConditionId")]
        public virtual StockCondition Condition { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierDetailId { get; set; }

        [Display(Name = "Supplier")]
        [ForeignKey("SupplierDetailId")]
        public virtual SupplierDetail Supplier { get; set; }

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
        [Display(Name = "Purchase Price")]
        public decimal? PurchasePrice { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public int? StockAuditDetailId { get; set; }

        [ForeignKey("StockAuditDetailId")]
        public StockAuditDetail AuditDetail { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (Quantity < 1) {
                yield return new ValidationResult("Quantity cannot be less than or equal to 0");
            }
        }

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =  false, NullDisplayText = "", DataFormatString = "{0:dd/mm/yy}")]
        public DateTime DateAdded { get; set; }
    }

}