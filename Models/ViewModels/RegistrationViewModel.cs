using System.ComponentModel.DataAnnotations;

namespace SimpleWebsite.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name="Имя")]
        public string Name {get; set;}

        [UIHint("email")]
        public string Email { get; set; }
        
        [UIHint("password")]
        [Display(Name="Пароль")]
        public string Password { get; set; }
    }
}