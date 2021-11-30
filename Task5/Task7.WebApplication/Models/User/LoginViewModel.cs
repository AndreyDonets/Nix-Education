using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.User
{
    public class LoginViewModel : BaseUserViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
