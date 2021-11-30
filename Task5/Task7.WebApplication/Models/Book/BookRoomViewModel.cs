using System;
using System.ComponentModel.DataAnnotations;
using Task7.WebApplication.Models.Room;

namespace Task7.WebApplication.Models.Book
{
    public class BookRoomViewModel : BaseRoomViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Patronymic { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 6)]
        public string Passport { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
