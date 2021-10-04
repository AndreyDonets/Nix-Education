using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager
{
    public class Context
    {
        public List<Visitor> Visitors { get; private set; }
        public List<HotelRoom> HotelRooms { get; private set; }
        public List<InternalHotelInformation> PrivateHotelInformation { get; private set; }
        private readonly JsonConverter converter = new JsonConverter();
        private readonly FileManager fileManager = new FileManager();
        public Context() => GetData();
        public void GetData()
        {
            Visitors = converter.Deserialize<Visitor>(fileManager.Read("Data\\visitors.json"));
            HotelRooms = converter.Deserialize<HotelRoom>(fileManager.Read("Data\\hotelRooms.json"));
            PrivateHotelInformation = converter.Deserialize<InternalHotelInformation>(fileManager.Read("Data\\privateHotelInformation.json"));
        }
        public void Save()
        {
            fileManager.Write(converter.Serialize(Visitors), "Data\\visitors.json");
            fileManager.Write(converter.Serialize(HotelRooms), "Data\\hotelRooms.json");
            fileManager.Write(converter.Serialize(PrivateHotelInformation), "Data\\privateHotelInformation.json");
        }
    }
}
