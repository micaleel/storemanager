using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models
{
    public class SupplierDetail {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Display(Name = "Supplier's Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Supplier's Email Address")]
        public string Email { get; set; }

        [Display(Name = "Supplier's Address")]
        public string Address { get; set; }
    }
}