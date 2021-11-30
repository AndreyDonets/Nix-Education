using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.Room
{
    public class CreateRoomViewModel : BaseRoomViewModel
    {

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string Category { get; set; }
    }
}
