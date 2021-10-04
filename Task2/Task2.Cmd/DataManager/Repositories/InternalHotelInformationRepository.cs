using System.Collections.Generic;
using System.Linq;
using Task2.Cmd.Interfaces;
using Task2.Cmd.Models;

namespace Task2.Cmd.DataManager.Repositories
{
    public class InternalHotelInformationRepository : IRepository<InternalHotelInformation>
    {
        private Context _context;
        public InternalHotelInformationRepository(Context context) => _context = context;
        public void Add(InternalHotelInformation entity)
        {
            if (GetById(entity.Id) == null)
                _context.PrivateHotelInformation.Add(entity);
        }
        public void Delete(InternalHotelInformation entity)
        {
            if (GetById(entity.Id) != null)
                _context.PrivateHotelInformation.Remove(entity);
        }
        public InternalHotelInformation GetById(int id) => _context.PrivateHotelInformation.FirstOrDefault(x => x.Id == id);
        public List<InternalHotelInformation> GetList() => _context.PrivateHotelInformation.ToList();
        public void Update(InternalHotelInformation entity)
        {
            Delete(entity);
            Add(entity);
        }
    }
}
