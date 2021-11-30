using System;
using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.Room
{
    public class RoomViewModel : CreateRoomViewModel
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
