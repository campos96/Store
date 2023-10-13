using Microsoft.EntityFrameworkCore;
using Store.Models;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    [Index(nameof(Email), nameof(Username), IsUnique = true)]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime? RegisterDate { get; set; }

        public Guid? PasswordResetToken {  get; set; }

        public DateTime? PasswordResetExpiration { get; set; }

        public DateTime? LastPasswordReset { get; set; }
    }
}
