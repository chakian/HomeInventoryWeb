using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.User
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name ="Password (repeat)")]
        public string PasswordCheck { get; set; }
    }
}
