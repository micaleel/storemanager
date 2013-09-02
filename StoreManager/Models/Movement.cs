using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models {

    public class Movement {
        public int Id { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public int StockId { get; set; }
        public int LocationId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
    }
}