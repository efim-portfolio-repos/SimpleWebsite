using System.ComponentModel.DataAnnotations;

namespace SimpleWebsite.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [UIHint("password")]
        [Display(Name="Пароль")]
        public string CurrentPassword { get; set; }
        
        [Required]
        [UIHint("password")]
        [Display(Name="Новый пароль")]
        public string NewPassword { get; set; }
    }
}