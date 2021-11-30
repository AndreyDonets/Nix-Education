using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Room
{
    public class CreateRoomViewModel : BaseRoomViewModel
    {

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string Category { get; set; }
    }
}
