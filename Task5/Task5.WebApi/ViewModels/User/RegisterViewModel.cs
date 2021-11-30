using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.User
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
