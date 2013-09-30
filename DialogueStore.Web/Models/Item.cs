using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DialogueStore.Web.Models {

    [DisplayColumn("Name")]
    public class Item {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item")]
        public string Name { get; set; }

        [Display(Name = "Re-Order Level")]
        public int ReorderLevel { get; set; }

        [Display(Name = "Is Discontinued")]
        public bool Discontinued { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Quantity in Stock")]
        public int QuantityinStock {
            get { return Stocks == null ? 0 : Stocks.Count; }
        }

        public virtual List<Stock> Stocks { get; set; }
    }
}