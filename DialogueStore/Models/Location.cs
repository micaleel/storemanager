using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DialogueStore.Models {

    public class Location {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Store")]
        public bool IsStore { get; set; }

        // Hack: the double navigation property is necessary for EF to create models.
        public virtual List<Movement> FromMovements { get; set; }
        public virtual List<Movement> ToMovements { get; set; }
    }
}