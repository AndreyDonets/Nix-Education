using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Room
{
    public class BaseRoomViewModel
    {
        [Range(0, int.MaxValue)]
        public int Number { get; set; }
    }
}
