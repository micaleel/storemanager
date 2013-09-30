using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DialogueStore.Web.ViewModels {
    public class RegisterModel {

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Secret Question")]
        public string SecretQuestion { get; set; }

        [Required]
        [DisplayName("Secret Answer")]
        public string SecretAnswer { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

    }
}