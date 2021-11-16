using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Room
{
    public class BaseRoomViewModel
    {
        [Required]
        public int Number { get; set; }
    }
}
