using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DialogueStore.Models {
    public class User {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }

        [Required]
        [DisplayName("Full Name")]
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string SecretAnswer { get; set; }
        public string SecretQuestion { get; set; }
        public bool Approved { get; set; }
        public bool Locked { get; set; }
        public DateTime LastLogin { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}