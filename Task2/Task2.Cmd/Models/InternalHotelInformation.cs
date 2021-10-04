using System;

namespace Task2.Cmd.Models
{
    public class InternalHotelInformation : BaseEntity
    {
        public int VisitorId { get; set; }
        public int HotelRoomId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? ReservedDate { get; set; }
    }
}
