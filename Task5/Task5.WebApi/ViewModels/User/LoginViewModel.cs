using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.User
{
    public class LoginViewModel : BaseUserViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
