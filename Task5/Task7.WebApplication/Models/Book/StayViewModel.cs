using System;
using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.Book
{
    public class StayViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public bool CheckedIn { get; set; }

        public bool CheckedOut { get; set; }

        [Range(0, int.MaxValue)]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 6)]
        public string Passport { get; set; }
    }
}
