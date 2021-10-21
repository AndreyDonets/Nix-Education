using System;
using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.Controllers;
using Task2.Cmd.DataManager;
using Task2.Cmd.Models;

namespace Task2.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork unit = new UnitOfWork();
            var hotelRoomController = new HotelRoomController(unit);
            var visitorController = new VisitorController(unit);
            var internalHotelInformationController = new InternalHotelInformationController(unit);
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Select operation:\n1 - work with data about hotel rooms\n2 - work with data about hotel visitors\n3 - check-in data, check-out data and room reservation\n4 - search for free rooms\nE - exit from the program");
                    var choose = Console.ReadKey();
                    if (choose.Key == ConsoleKey.E)
                        break;
                    switch (choose.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();
                            Console.WriteLine("Select operation:\n1 - add hotel room\n2 - search hotel room\n3 - delete hotel room\nanother key - return to the main menu");
                            var allHotelRooms = hotelRoomController.GetAllHotelRooms();
                            switch (Console.ReadKey().Key)
                            {
                                #region add hotel room
                                case ConsoleKey.D1:
                                    var numberHotelRoom = 0;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter room number or \"exit\" for return to the main menu");
                                        var stringNumberHotelRoom = Console.ReadLine();
                                        if (stringNumberHotelRoom.ToLower() == "exit")
                                            break;
                                        if (int.TryParse(stringNumberHotelRoom, out numberHotelRoom))
                                        {
                                            if (!allHotelRooms.Any(x => x.Number == numberHotelRoom))
                                                break;
                                            else
                                                Console.WriteLine("A hotel room with the same number already exists\nPress any key");
                                        }
                                        else
                                            Console.WriteLine("Invalid input\nPress any key");
                                        Console.ReadKey();
                                    }
                                    if (numberHotelRoom == 0)
                                        break;
                                    var categoryHotelRoom = new CategoriesHotelRoom();
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Choose category hotel room:");
                                        for (int i = 1; i <= 8; i++)
                                            Console.WriteLine($"{i} - {(CategoriesHotelRoom)i}");
                                        Console.WriteLine("9 - for return to the main menu");
                                        if (int.TryParse(Console.ReadLine(), out int categoryNumber) && categoryNumber > 0 && categoryNumber > 9)
                                        {
                                            if (categoryNumber == 9)
                                                break;
                                            categoryHotelRoom = (CategoriesHotelRoom)categoryNumber;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Select a value from the list\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (categoryHotelRoom == 0)
                                        break;
                                    decimal priceHotelRoom = 0;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter price hotel room or \"exit\" for return to the main menu");
                                        var stringPrice = Console.ReadLine();
                                        if (stringPrice.ToLower() == "exit")
                                            break;
                                        if (decimal.TryParse(stringPrice, out priceHotelRoom))
                                            break;
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (priceHotelRoom == 0)
                                        break;
                                    Console.Clear();
                                    hotelRoomController.AddHotelRoom(numberHotelRoom, categoryHotelRoom, priceHotelRoom);
                                    var s = hotelRoomController.GetAllHotelRooms();
                                    Console.WriteLine("Hotel room added successfully\nPress any key");
                                    Console.ReadKey();
                                    break;
                                #endregion
                                #region search hotel room
                                case ConsoleKey.D2:
                                    Console.Clear();
                                    Console.WriteLine("Select operation:\n1 - search by id\n2 - search by number\n3 - display the entire list of numbers\nanother key - return to the main menu");
                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.D1:
                                            Console.Clear();
                                            Console.WriteLine("Enter id hotel room");
                                            if (int.TryParse(Console.ReadLine(), out int idHotelRoom))
                                            {
                                                var hotelRoom = hotelRoomController.GetByIdHotelRoom(idHotelRoom);
                                                if (hotelRoom != null)
                                                    Console.WriteLine($"Id hotel room - {hotelRoom.Id}\nNumber hotel room - {hotelRoom.Number}\nCategory hotel room - {hotelRoom.Category}\nPrice - {hotelRoom.Price}");
                                                else
                                                    Console.WriteLine("With this ID the hotel room was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D2:
                                            Console.Clear();
                                            Console.WriteLine("Enter number hotel room");
                                            if (int.TryParse(Console.ReadLine(), out numberHotelRoom))
                                            {
                                                var hotelRoom = hotelRoomController.GetByNumberHotelRoom(numberHotelRoom);
                                                if (hotelRoom != null)
                                                    Console.WriteLine($"Id hotel room - {hotelRoom.Id}\nNumber hotel room - {hotelRoom.Number}\nCategory hotel room - {hotelRoom.Category}\nPrice - {hotelRoom.Price}");
                                                else
                                                    Console.WriteLine("With this number the hotel room was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D3:
                                            Console.Clear();
                                            foreach (var hotelRoom in allHotelRooms)
                                                Console.WriteLine($"Id hotel room - {hotelRoom.Id}\nNumber hotel room - {hotelRoom.Number}\nCategory hotel room - {hotelRoom.Category}\nPrice - {hotelRoom.Price}\n");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                    }
                                    break;
                                #endregion
                                #region delete hotel room
                                case ConsoleKey.D3:
                                    Console.Clear();
                                    Console.WriteLine("Select operation:\n1 - delete by id\n2 - delete by number\n3 - select from list and remove");
                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.D1:
                                            Console.Clear();
                                            Console.WriteLine("Enter id hotel room");
                                            if (int.TryParse(Console.ReadLine(), out int idHotelRoom))
                                            {
                                                var hotelRoom = hotelRoomController.GetByIdHotelRoom(idHotelRoom);
                                                if (hotelRoom != null)
                                                {
                                                    hotelRoomController.DeleteHotelRoom(hotelRoom);
                                                    Console.WriteLine("Hotel room deleted successfully");
                                                }
                                                else
                                                    Console.WriteLine("With this ID the hotel room was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D2:
                                            Console.Clear();
                                            Console.WriteLine("Enter number hotel room");
                                            if (int.TryParse(Console.ReadLine(), out numberHotelRoom))
                                            {
                                                var hotelRoom = hotelRoomController.GetByNumberHotelRoom(numberHotelRoom);
                                                if (hotelRoom != null)
                                                {
                                                    hotelRoomController.DeleteHotelRoom(hotelRoom);
                                                    Console.WriteLine("Hotel room deleted successfully");
                                                }
                                                else
                                                    Console.WriteLine("With this number the hotel room was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D3:
                                            Console.Clear();
                                            Console.WriteLine("Select a hotel room from the list");
                                            for (int i = 0; i < allHotelRooms.Count; i++)
                                                Console.WriteLine($"{i + 1}: Id hotel room - {allHotelRooms[i].Id}, Number hotel room - {allHotelRooms[i].Number}, Category hotel room - {allHotelRooms[i].Category}, Price - {allHotelRooms[i].Price}");
                                            if (int.TryParse(Console.ReadLine(), out int chooseId) && chooseId > 0 && allHotelRooms.Count >= chooseId)
                                            {
                                                hotelRoomController.DeleteHotelRoom(allHotelRooms[chooseId - 1]);
                                                Console.WriteLine("Hotel room deleted successfully");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                    }
                                    break;
                                    #endregion
                            }
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Console.WriteLine("Select operation:\n1 - add visitor\n2 - search visitor information\n3 - delete visitor");
                            var allVisitors = visitorController.GetAllVisitor();
                            switch (Console.ReadKey().Key)
                            {
                                #region add visitor
                                case ConsoleKey.D1:
                                    Console.Clear();
                                    Console.WriteLine("Enter the name of the visitor");
                                    var firstName = Console.ReadLine();
                                    Console.WriteLine("Enter the surname of the visitor");
                                    var surName = Console.ReadLine();
                                    Console.WriteLine("Enter the patronymic of the visitor");
                                    var patronymic = Console.ReadLine();
                                    Console.WriteLine("Enter the pasport number of the visitor");
                                    var pasportNumber = Console.ReadLine();
                                    DateTime birthDate = new DateTime();
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter the birthDate of the visitor dd.mm.yyyy or \"exit\" for return to the main menu");
                                        var stringBirthDate = Console.ReadLine();
                                        if (stringBirthDate.ToLower() == "exit")
                                            break;
                                        if (DateTime.TryParse(stringBirthDate, out birthDate))
                                            break;
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (birthDate == new DateTime())
                                        break;
                                    GenderType gender = new GenderType();
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Choose gender of the visitor:");
                                        for (int i = 1; i <= 2; i++)
                                            Console.WriteLine($"{i} - {(GenderType)i}");
                                        Console.WriteLine("3 - for return to the main menu");
                                        if (int.TryParse(Console.ReadLine(), out int genderNumber) && genderNumber > 0 && genderNumber <= 3)
                                        {
                                            if (genderNumber == 3)
                                                break;
                                            gender = (GenderType)genderNumber;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Select a value from the list\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (gender == 0)
                                        break;
                                    visitorController.AddVisitor(firstName, surName, patronymic, pasportNumber, birthDate, gender);
                                    Console.WriteLine("Visitor added successfully\nPress any key");
                                    Console.ReadKey();
                                    break;
                                #endregion
                                #region search visitor
                                case ConsoleKey.D2:
                                    Console.Clear();
                                    Console.WriteLine("Select operation:\n1 - search by id\n2 - search by pasport number\n3 - display the entire list of numbers");
                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.D1:
                                            Console.Clear();
                                            Console.WriteLine("Enter id visitor");
                                            var visitor = new Visitor();
                                            if (int.TryParse(Console.ReadLine(), out int idVisitor))
                                            {
                                                visitor = visitorController.GetByIdVisitor(idVisitor);
                                                if (visitor != null)
                                                    Console.WriteLine($"Id visitor - {visitor.Id}\nName visitor - {visitor.SurName} {visitor.FirstName} {visitor.Patronymic}\nGender visitor - {visitor.Gender}\nPasport number visitor - {visitor.PasportNumber}");
                                                else
                                                    Console.WriteLine("With this ID the visitor was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D2:
                                            Console.Clear();
                                            Console.WriteLine("Enter pasport number visitor");
                                            pasportNumber = Console.ReadLine();
                                            visitor = visitorController.GetByPasportNumberVisitor(pasportNumber);
                                            if (visitor != null)
                                                Console.WriteLine($"Id visitor - {visitor.Id}\nName visitor - {visitor.SurName} {visitor.FirstName} {visitor.Patronymic}\nGender visitor - {visitor.Gender}\nPasport number visitor - {visitor.PasportNumber}");
                                            else
                                                Console.WriteLine("With this pasport number the visitor was not found");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D3:
                                            Console.Clear();
                                            foreach (var vis in allVisitors)
                                                Console.WriteLine($"Id visitor - {vis.Id}\nName visitor - {vis.SurName} {vis.FirstName} {vis.Patronymic}\nGender visitor - {vis.Gender}\nPasport number visitor - {vis.PasportNumber}\n");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                    }
                                    break;
                                #endregion
                                #region delete visitor
                                case ConsoleKey.D3:
                                    Console.Clear();
                                    Console.WriteLine("Select operation:\n1 - delete by id\n2 - delete by pasport number\n3 - select from list and remove");
                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.D1:
                                            Console.Clear();
                                            Console.WriteLine("Enter id visitor");
                                            var visitor = new Visitor();
                                            if (int.TryParse(Console.ReadLine(), out int idVisitor))
                                            {
                                                visitor = visitorController.GetByIdVisitor(idVisitor);
                                                if (visitor != null)
                                                {
                                                    visitorController.DeleteVisitor(visitor);
                                                    Console.WriteLine("Visitor deleted successfully");
                                                }
                                                else
                                                    Console.WriteLine("With this ID the visitor was not found");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D2:
                                            Console.Clear();
                                            Console.WriteLine("Enter pasport number visitor");
                                            pasportNumber = Console.ReadLine();
                                            visitor = visitorController.GetByPasportNumberVisitor(pasportNumber);
                                            if (visitor != null)
                                            {
                                                visitorController.DeleteVisitor(visitor);
                                                Console.WriteLine("Visitor deleted successfully");
                                            }
                                            else
                                                Console.WriteLine("With this pasport number the visitor was not found");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                        case ConsoleKey.D3:
                                            Console.Clear();
                                            Console.WriteLine("Select a visitor from the list");
                                            for (int i = 0; i < allVisitors.Count; i++)
                                                Console.WriteLine($"{i + 1}: Id visitor - {allVisitors[i].Id}, Name visitor - {allVisitors[i].SurName} {allVisitors[i].FirstName} {allVisitors[i].Patronymic}, Gender visitor - {allVisitors[i].Gender}, Pasport number visitor - {allVisitors[i].PasportNumber}");
                                            if (int.TryParse(Console.ReadLine(), out int chooseId) && chooseId > 0 && allVisitors.Count >= chooseId)
                                            {
                                                visitorController.DeleteVisitor(allVisitors[chooseId - 1]);
                                                Console.WriteLine("Visitor deleted successfully");
                                            }
                                            else
                                                Console.WriteLine("Invalid input");
                                            Console.WriteLine("Press any key");
                                            Console.ReadKey();
                                            break;
                                    }
                                    break;
                                    #endregion
                            }
                            break;
                        case ConsoleKey.D3:
                            Console.Clear();
                            Console.WriteLine("Select operation:\n1 - reserve a hotel room\n2 - check in at the hotel\n3 - check out the hotel");
                            switch (Console.ReadKey().Key)
                            {
                                #region reserve room
                                case ConsoleKey.D1:
                                    var visitorId = 0;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("To book a room enter your passport number or \"exit\" for return to the main menu");
                                        var pasportNumber = Console.ReadLine();
                                        if (pasportNumber.ToLower() == "exit")
                                            break;
                                        var visitor = visitorController.GetByPasportNumberVisitor(pasportNumber);
                                        if (visitor != null)
                                        {
                                            visitorId = visitor.Id;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (visitorId == 0)
                                        break;
                                    var hotelRoomId = 0;
                                    var reservedDate = new DateTime();
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter the date on which you want to book a room dd.mm.yyyy or \"exit\" for return to the main menu");
                                        var stringReservedDate = Console.ReadLine();
                                        if (stringReservedDate.ToLower() == "exit")
                                            break;
                                        if (DateTime.TryParse(stringReservedDate, out reservedDate) && reservedDate >= DateTime.Today)
                                        {
                                            var hotelRoom = ChooseFreeHotelRoom(hotelRoomController.GetAllHotelRooms(), internalHotelInformationController.GetAllHotelInformation(), reservedDate);
                                            if (hotelRoom == null)
                                                break;
                                            hotelRoomId = hotelRoom.Id;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (reservedDate == new DateTime() || hotelRoomId == 0)
                                        break;
                                    internalHotelInformationController.ReserveHotelRoom(visitorId, hotelRoomId, reservedDate);
                                    Console.WriteLine("Hotel room was successfully reserved\nPress any key");
                                    Console.ReadKey();
                                    break;
                                #endregion
                                #region check into the room
                                case ConsoleKey.D2:
                                    while (true)
                                    {
                                        visitorId = 0;
                                        Console.Clear();
                                        Console.WriteLine("To check in, enter your passport number or \"exit\" for return to the main menu");
                                        var pasportNumber = Console.ReadLine();
                                        if (pasportNumber.ToLower() == "exit")
                                            break;
                                        var visitor = visitorController.GetByPasportNumberVisitor(pasportNumber);
                                        if (visitor != null)
                                        {
                                            visitorId = visitor.Id;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (visitorId == 0)
                                        break;
                                    var reserve = internalHotelInformationController.GetAllHotelInformation().FirstOrDefault(x => x.VisitorId == visitorId && x.ReservedDate.HasValue && x.ReservedDate.Value.Date == DateTime.Today);
                                    if (reserve != null)
                                        internalHotelInformationController.Registration(reserve);
                                    else
                                    {
                                        Console.Clear();
                                        var hotelRoom = ChooseFreeHotelRoom(hotelRoomController.GetAllHotelRooms(), internalHotelInformationController.GetAllHotelInformation(), DateTime.Today);
                                        if (hotelRoom == null)
                                            break;
                                        hotelRoomId = hotelRoom.Id;
                                        internalHotelInformationController.Registration(visitorId, hotelRoomId);
                                    }
                                    Console.WriteLine("Check-in was successful\nPress any key");
                                    Console.ReadKey();
                                    break;
                                #endregion
                                #region check out the hotel
                                case ConsoleKey.D3:
                                    while (true)
                                    {
                                        visitorId = 0;
                                        Console.Clear();
                                        Console.WriteLine("To check out, enter your passport number or \"exit\" for return to the main menu");
                                        var pasportNumber = Console.ReadLine();
                                        if (pasportNumber.ToLower() == "exit")
                                            break;
                                        var visitor = visitorController.GetByPasportNumberVisitor(pasportNumber);
                                        if (visitor != null)
                                        {
                                            visitorId = visitor.Id;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input\nPress any key");
                                            Console.ReadKey();
                                        }
                                    }
                                    if (visitorId == 0)
                                        break;
                                    var registration = internalHotelInformationController.GetAllHotelInformation().FirstOrDefault(x => x.VisitorId == visitorId && x.RegistrationDate.HasValue);
                                    internalHotelInformationController.Eviction(registration);
                                    break;
                                    #endregion
                            }
                            break;
                        case ConsoleKey.D4:
                            Console.Clear();
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the date on which you want to book a room dd.mm.yyyy or \"exit\" for return to the main menu");
                                var stringDate = Console.ReadLine();
                                if (stringDate.ToLower() == "exit")
                                    break;
                                if (DateTime.TryParse(stringDate, out var date) && date >= DateTime.Today)
                                {
                                    var hotelRoom = ChooseFreeHotelRoom(hotelRoomController.GetAllHotelRooms(), internalHotelInformationController.GetAllHotelInformation(), date);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input\nPress any key");
                                    Console.ReadKey();
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                }
            }
        }
        private static HotelRoom ChooseFreeHotelRoom(List<HotelRoom> hotelRooms, List<InternalHotelInformation> roomInformation, DateTime date)
        {
            var idOccupiedHotelRooms = roomInformation
                .Where(x => (x.RegistrationDate.HasValue && !x.CheckOutDate.HasValue) || (x.ReservedDate.HasValue && x.ReservedDate.Value.Date == date.Date))
                .Select(x => x.HotelRoomId);
            var freeRooms = hotelRooms.Where(x => idOccupiedHotelRooms.All(y => y != x.Id)).ToList();
            if (freeRooms.Count == 0)
            {
                Console.WriteLine("Unfortunately, there are no rooms available at the moment\nPress any key");
                Console.ReadKey();
                return null;
            }
            while (true)
            {
                Console.WriteLine("List of available hotel rooms, please select:");
                for (int i = 0; i < freeRooms.Count; i++)
                    Console.WriteLine($"{i + 1}: Id hotel room - {freeRooms[i].Id}, Number hotel room - {freeRooms[i].Number}, Category hotel room - {freeRooms[i].Category}, Price - {freeRooms[i].Price}");
                if (int.TryParse(Console.ReadLine(), out int choose) && choose > 0 && freeRooms.Count >= choose)
                    return freeRooms[choose - 1];
                else
                {
                    Console.WriteLine("Invalid input\nPress any key");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
