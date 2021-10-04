namespace Task2.Cmd.Models
{
    public enum CategoriesHotelRoom
    {
        Standart = 1,
        Balcony,
        Studio,
        Luxe,
        Business,
        Duplex,
        Apartment,
        President
    }
    public class HotelRoom : BaseEntity
    {
        
        public int Number { get; set; }
        public CategoriesHotelRoom Category { get; set; }
        public decimal Price { get; set; }
    }
}
