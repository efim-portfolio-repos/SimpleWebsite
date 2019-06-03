using System.ComponentModel.DataAnnotations;

namespace SimpleWebsite.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name="Пароль")]
        public string Password {get; set;}
    }
}