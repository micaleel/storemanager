using System.ComponentModel.DataAnnotations;

namespace DialogueStore.Web.ViewModels
{
    public class LoginViewModel {
        public string UserName { get; set; }
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}