using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreManager.Models {

    public class Location {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Store")]
        public bool IsStore { get; set; }
        public virtual List<Movement> Movements { get; set; }
    }
}