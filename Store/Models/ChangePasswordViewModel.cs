using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DisplayName("Confirm New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }


    }
}
