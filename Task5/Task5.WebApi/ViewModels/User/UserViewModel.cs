using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.User
{
    public class UserViewModel : BaseUserViewModel
    {

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
