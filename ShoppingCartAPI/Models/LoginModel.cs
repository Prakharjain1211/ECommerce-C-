using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Model
{
    public class LoginModel
    {
        [EmailAddress]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}