using System;

namespace Task5.WebApi.ViewModels.Book
{
    public class StayViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CheckedIn { get; set; }
        public bool CheckedOut { get; set; }
        public int RoomNumber { get; set; }
        public string Passport { get; set; }
    }
}
