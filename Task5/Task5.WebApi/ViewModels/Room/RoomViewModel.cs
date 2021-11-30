using System;
using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Room
{
    public class RoomViewModel : CreateRoomViewModel
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
