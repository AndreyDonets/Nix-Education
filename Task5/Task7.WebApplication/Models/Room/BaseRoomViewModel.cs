using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.Room
{
    public class BaseRoomViewModel
    {
        [Range(0, int.MaxValue)]
        public int Number { get; set; }
    }
}
