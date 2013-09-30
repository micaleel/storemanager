using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DialogueStore.Web.Models
{
    public class ItemAuditDetail {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [Display(Name = "Time Created")]
        [DataType(DataType.Date)]
        public DateTime? CreatedOn { get; set; }

        public int CreatedByUserId { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByFullName { get; set; }

        [Display(Name = "Last Modification")]
        public DateTime? ModifiedOn { get; set; }

        public int ModifiedByUserId { get; set; }

        [Display(Name = "Last Modified By")]
        public string ModifiedByFullName { get; set; }
    }
}