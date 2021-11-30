using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Room
{
    public class ChangeRoomViewModel : BaseRoomViewModel
    {

        [Range(0, int.MaxValue)]
        public int NewNumber { get; set; }

        [MaxLength(50)]
        public string NewCategory { get; set; }
    }
}
