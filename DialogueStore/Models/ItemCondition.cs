using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DialogueStore.Models {

    [DisplayColumn("Name")]
    public class StockCondition {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Stock Condition")]
        public string Name { get; set; }
    }
}